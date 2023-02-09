using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Rotate_Around : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform sun;

    void Update()
    {
        transform.RotateAround(sun.position, transform.TransformDirection(Vector3.up), speed * Time.deltaTime);
    }
}
