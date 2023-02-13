using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pen : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    private Vector3 trailLastEmittingPosition;

    void Start()
    {        
        trail.emitting = false;
    }


    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.08f))
        {            
            Vector3 offset = hit.normal.normalized * 0.002f;
            trail.transform.position = hit.point + offset;
            trailLastEmittingPosition = hit.point + offset;
            trail.emitting = true;
        }
        else
        {
            trail.emitting = false;
            trail.transform.position = trailLastEmittingPosition;
        }  
    }
}
