using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDirectionalCollider : MonoBehaviour
{
    [SerializeField] private Cube cube;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Saber_Blue")
        {
            cube.TouchedFromRightDirection("Saber_Blue");
        }
        else if (other.name == "Saber_Red")
        {
            cube.TouchedFromRightDirection("Saber_Red");
        }
    }
}
