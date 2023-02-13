using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public enum cubeColor { Red, Blue }


public class Cube : MonoBehaviour
{
    public cubeColor color { get; private set; }

    public float speed = 0.1f;
    [Space(10)]
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Material redMat;
    [SerializeField] private Material blueMat;
    private Beat_Saber_GM gameManager;

    private bool isTouchedFromRightDirection;


    void Start()
    {
        ChoseColor();

        gameManager = GameObject.Find("Game Manager").GetComponent<Beat_Saber_GM>();

        transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f), 10);
    }

    void Update()
    {
        CubeMove();
        CheckBoundary();
    }

    private void CheckBoundary()
    {
        if (transform.position.z < -1)
        {
            Destroy(gameObject);
            gameManager.UpdateScore(-1);
        }
    }

    private void ChoseColor()
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

    public void TouchedFromRightDirection(string saber)
    {
        if (saber == "Saber_Red" && color == cubeColor.Red) isTouchedFromRightDirection = true;
        else if (saber == "Saber_Blue" && color == cubeColor.Blue) isTouchedFromRightDirection = true;
        StartCoroutine(WaitForCancelTouched());
    }

    IEnumerator WaitForCancelTouched()
    {
        yield return new WaitForSeconds(0.5f);
        isTouchedFromRightDirection = false;
    }


    public void CubeMove()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Saber_Blue" && color == cubeColor.Blue && isTouchedFromRightDirection)
        {
            Destroy(gameObject);
            gameManager.UpdateScore(1);
        }
        else if (other.name == "Saber_Red" && color == cubeColor.Red && isTouchedFromRightDirection)
        {
            Destroy(gameObject);
            gameManager.UpdateScore(1);
        }
    }
}
