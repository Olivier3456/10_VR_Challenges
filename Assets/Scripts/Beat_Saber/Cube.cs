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

    private Beat_Saber_GM gameManager;

    public bool isTouched;
    public bool isTouchedFromRightDirection;
    public bool isActive = true;                    // isActive nous sert juste à ne pas déclencher plusieurs fois le son wrongBell.

    public CubeSpawner cubeSpawner;


    void Start()
    {
        audioSource = GameObject.Find("CubeSounds").GetComponent<AudioSource>();

        cubeSpawner = GameObject.Find("CubeSpawner").GetComponent<CubeSpawner>();

        gameManager = GameObject.Find("Game Manager").GetComponent<Beat_Saber_GM>();

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
        cubeSpawner.AddCubeToDisabledList(transform.gameObject);
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

    //public void TouchedByRaycast(string saber, Transform emitterPosition)
    //{
    //    if (!isTouched)
    //    {
    //        if ((saber == "Saber_Red" && color == cubeColor.Red) || (saber == "Saber_Blue" && color == cubeColor.Blue))
    //        {
    //            if (orientation == 0 && emitterPosition.position.y + transform.localScale.y * 0.5f < transform.position.y)      // Si le sabre est dans la direction de la flèche.
    //            { isTouchedFromRightDirection = true; }
    //            else if (orientation == 90 && emitterPosition.position.x - transform.localScale.x * 0.5f > transform.position.x)
    //            { isTouchedFromRightDirection = true; }
    //            else if (orientation == 180 && emitterPosition.position.y - transform.localScale.y * 0.5f > transform.position.y)
    //            { isTouchedFromRightDirection = true; }
    //            else if (orientation == 270 && emitterPosition.position.x + transform.localScale.x * 0.5f < transform.position.x)
    //            { isTouchedFromRightDirection = true; }               
    //        }
    //        isTouched = true;
    //    }
    //}



    public void TouchedByRaycast(string saber, Transform emitterPosition)
    {
        if (!isTouched)
        {
            if ((saber == "Saber_Red" && color == cubeColor.Red) || (saber == "Saber_Blue" && color == cubeColor.Blue))
            {
                // Normalise les vecteurs pour nous assurer qu'ils ont une longueur égale à 1
                Vector3 normalizedCubeDirection = transform.forward.normalized;
                Vector3 normalizedSabreMovementDirection = emitterPosition.position.normalized; // Attention, il faudra remplacer emitterPosition, qui ne correspond pas à une direction : il faudra faire passer la direction de RaycastController.

                // Projette les vecteurs sur le plan du cube en fixant l'axe y à zéro
                Vector3 projectedCubeDirection = Vector3.ProjectOnPlane(normalizedCubeDirection, Vector3.forward);           // Attention, il ne s'agira peut-être pas de Vector3.forward. A voir en test.
                Vector3 projectedSabreMovementDirection = Vector3.ProjectOnPlane(normalizedSabreMovementDirection, Vector3.forward);

                // Calcule l'angle entre les deux vecteurs projetés sur le plan horizontal
                // float angle = Vector3.Angle(projectedCubeDirection, projectedSabreMovementDirection);
                float signedAngle = Vector3.SignedAngle(projectedCubeDirection, projectedSabreMovementDirection, Vector3.forward);  // Changer forward ici aussi si besoin.

                // Ajuste l'angle pour obtenir un angle entre 0 et 360 degrés
                float angle = (signedAngle < 0) ? 360 + signedAngle : signedAngle;


                Debug.Log("Angle entre les deux directions : " + signedAngle + " degrés");
            }
            isTouched = true;
        }
    }





    public void CubeMove()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
    }
}
