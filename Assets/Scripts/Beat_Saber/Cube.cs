using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public enum cubeColor { Red, Blue }

public class Cube : MonoBehaviour
{
    public cubeColor color { get; private set; }

    [SerializeField] private float speed = 0.1f;
    [Space(10)]
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Material redMat;
    [SerializeField] private Material blueMat;
    private Beat_Saber_GM gameManager;


    void Start()
    {
        ChoseColor();

        gameManager = GameObject.Find("Game Manager").GetComponent<Beat_Saber_GM>();

        transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f), 5);

        float newScale = Random.Range(0.15f, 0.3f);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    void Update()
    {
        CubeMove();

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


    public void CubeMove()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Qqch est entré dans le trigger d'un cube.");
        if (other.CompareTag("Blue_Saber") && color == cubeColor.Blue)
        {
            Destroy(gameObject);
            gameManager.UpdateScore(1);
        }
        else if (other.CompareTag("Red_Saber") && color == cubeColor.Red)
        {
            Destroy(gameObject);
            gameManager.UpdateScore(1);
        }
        else if (other.CompareTag("Red_Saber") && color == cubeColor.Blue)
        {
            gameManager.UpdateScore(-1);
        }
        else if (other.CompareTag("Blue_Saber") && color == cubeColor.Red)
        {
            gameManager.UpdateScore(-1);
        }
    }
}
