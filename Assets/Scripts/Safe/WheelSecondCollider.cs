using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSecondCollider : MonoBehaviour
{
    public bool isRightHand { get; private set; }
    public bool isLeftHand { get; private set; }

    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == rightHand.name)
        {
            isRightHand = true;
            Debug.Log(other.name + " entered in the Second wheel collider");
        }
            

        else if (other.name == leftHand.name)
        {
            isLeftHand = true;
            Debug.Log(other.name + " entered in the Second wheel collider");
        }
            

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == rightHand.name)
        {
            isRightHand = false;            
        }
            

        else if (other.name == leftHand.name)
        {
            isLeftHand = false;            
        }
            
    }
}
