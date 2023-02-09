using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SolarSystem : MonoBehaviour
{
    [SerializeField] private Transform[] attachPoints;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [SerializeField] private XRGrabInteractable xrGrab;
    private Transform actualAttachPoint;


    public void grabbed()
    {
        float mindistanceFromHand = 1000;

        for (int i = 0; i < attachPoints.Length; i++)
        {
            float distanceFromHand = Vector3.Distance(attachPoints[i].position, rightHand.position);           
            if (distanceFromHand < mindistanceFromHand)
            {
                mindistanceFromHand = distanceFromHand;
                actualAttachPoint = attachPoints[i];
            }
        }
        xrGrab.attachTransform = actualAttachPoint;
    }


   
}
