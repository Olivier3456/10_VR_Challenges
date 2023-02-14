using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberPoint : MonoBehaviour
{
    public Vector3 lastPosition { get; private set; }
    public Vector3 actualPosition { get; private set; }
    void Awake()
    {
        lastPosition = transform.position;
        actualPosition = transform.position;
    }

    
    void FixedUpdate()
    {
        lastPosition = actualPosition;
        actualPosition = transform.position;
    }
}
