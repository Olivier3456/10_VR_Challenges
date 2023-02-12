using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PenVFXGraph : MonoBehaviour
{

    [SerializeField] private VisualEffect trailGraph;
    
    
    void Start()
    {
        //   trailGraph.SetBool("Emitting", true);
        trailGraph.SetBool("WorldSpace", true);
        trailGraph.Play();

    }

        
}
