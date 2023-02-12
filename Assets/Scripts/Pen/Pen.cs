using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    private int colliders;

    void Start()
    {
        trail.emitting = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("On collision Enter");

        if (collision.gameObject.CompareTag("Markable"))
        {
            colliders++;            
        }

        if (colliders >= 2)
        {
            trail.emitting = true;
        }        
    }


    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("On collision Exit");

        if (collision.gameObject.CompareTag("Markable"))
        {
            colliders--;
            trail.emitting = false;
        }        
    }
}
