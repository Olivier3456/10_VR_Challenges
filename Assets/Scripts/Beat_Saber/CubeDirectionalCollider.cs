using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDirectionalCollider : MonoBehaviour
{
    [SerializeField] private Cube cube;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " est entré dans le trigger d'un CubeDirectionalCollider");

        if (other.CompareTag("Blue_Saber"))
        {
            cube.TouchedFromRightDirection("Saber_Blue");
            Debug.Log("CubeDirectionalCollider appelle cube.TouchedFromRightDirection(\"Saber_Blue\")");
        }
        else if (other.CompareTag("Red_Saber"))
        {
            cube.TouchedFromRightDirection("Saber_Red");
            Debug.Log("CubeDirectionalCollider appelle cube.TouchedFromRightDirection(\"Saber_Red\")");
        }
    }
    
    public void TouchedByRaycast(string saberName)
    {
        cube.TouchedFromRightDirection(saberName);
    }
}
