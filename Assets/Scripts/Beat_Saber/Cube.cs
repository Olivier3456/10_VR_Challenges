using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using static Unity.VisualScripting.Member;

public enum cubeColor { Red, Blue }
public enum cubeOrientation { Left, Right, Top, Down }


public class Cube : MonoBehaviour
{
    public cubeColor color { get; private set; }

    public int orientation;

    public float speed = 0.1f;
    [Space(10)]
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Material redMat;
    [SerializeField] private Material blueMat;
    [SerializeField] private Material blackMat;
    [Space(5)]
    private AudioSource audioSource;
    [SerializeField] private AudioClip goodBell;
    [SerializeField] private AudioClip wrongBell;
    [Space(5)]
    [SerializeField] private float toleranceAngle = 40;

    private Beat_Saber_GM gameManager;

    private BoxCollider boxCollider;

    public bool isTouched;
    public bool isTouchedFromRightDirection;
    public bool isActive = true;                    // isActive nous sert juste à ne pas déclencher plusieurs fois le son wrongBell.

    public CubeSpawner cubeSpawner;


    void Start()
    {
        audioSource = GameObject.Find("CubeSounds").GetComponent<AudioSource>();

        cubeSpawner = GameObject.Find("CubeSpawner").GetComponent<CubeSpawner>();

        gameManager = GameObject.Find("Game Manager").GetComponent<Beat_Saber_GM>();

        boxCollider = GetComponent<BoxCollider>();

        transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

        gameObject.SetActive(false);
    }

    void Update()
    {
        CubeMove();
        CheckBoundary();
        if (isActive) CheckIfTouched();
    }

    private void CheckIfTouched()
    {
        if (isTouchedFromRightDirection)
        {
            audioSource.clip = goodBell;
            audioSource.Play();
            gameManager.UpdateScore(1);
            gameObject.SetActive(false);
        }
        else if (isTouched)
        {
            audioSource.clip = wrongBell;
            audioSource.Play();
            isActive = false;
            meshRenderer.material = blackMat;
        }
    }


    private void OnDisable()
    {
        cubeSpawner.AddCubeToDisabledList(transform.gameObject, this, boxCollider);
    }


    private void CheckBoundary()
    {
        if (transform.position.z < -1)
        {
            gameObject.SetActive(false);
            gameManager.UpdateScore(-1);
        }
    }

    public void ChoseColor()
    {
        int randomColor = Random.Range(0, 2);
        switch (randomColor)
        {
            case 0:
                color = cubeColor.Blue;
                meshRenderer.material = blueMat;
                break;
            case 1:
                color = cubeColor.Red;
                meshRenderer.material = redMat;
                break;
        }
    }

    public void TouchedByRaycast(string saber, Transform emitterPosition)       // (Ancienne méthode) comparaison des positions. Pas très élégante, mais fonctionne plutôt bien.
    {
        if (!isTouched)
        {
            if ((saber == "Saber_Red" && color == cubeColor.Red) || (saber == "Saber_Blue" && color == cubeColor.Blue))
            {
                if (orientation == 0 && emitterPosition.position.y + transform.localScale.y * 0.5f < transform.position.y)      // Si le sabre est dans la direction de la flèche.
                { isTouchedFromRightDirection = true; }
                else if (orientation == 90 && emitterPosition.position.x - transform.localScale.x * 0.5f > transform.position.x)
                { isTouchedFromRightDirection = true; }
                else if (orientation == 180 && emitterPosition.position.y - transform.localScale.y * 0.5f > transform.position.y)
                { isTouchedFromRightDirection = true; }
                else if (orientation == 270 && emitterPosition.position.x + transform.localScale.x * 0.5f < transform.position.x)
                { isTouchedFromRightDirection = true; }
            }
            isTouched = true;
        }
    }


    public void TouchedByRaycast(string saber, Vector3 emitterDirection)   // (Nouvelle méthode) comparaison des angles, avec projection sur un même plan. Plus précis et paramétrable (angle de tolérance réglable).
    {
        if (!isTouched)
        {
            if ((saber == "Saber_Red" && color == cubeColor.Red) || (saber == "Saber_Blue" && color == cubeColor.Blue))
            {
                // Le plan sur lequel les deux vecteurs seront projetés (le plan forward du cube) :
                Vector3 normal = transform.forward.normalized;

                // Projection de la direction du cube et de la direction du sabre sur le plan :
                Vector3 projectedCubeDirection = Vector3.ProjectOnPlane(transform.up, normal);
                Vector3 projectedSabreDirection = Vector3.ProjectOnPlane(emitterDirection, normal);

                // Calcul de l'angle entre les deux vecteurs projetés :
                float angle = Vector3.Angle(projectedCubeDirection, projectedSabreDirection);

                if (angle < toleranceAngle)
                {
                    isTouchedFromRightDirection = true;                    
                }
            }
            isTouched = true;
            boxCollider.enabled = false;
        }
    }


    public void CubeMove()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
    }
}
