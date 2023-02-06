using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{    
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    private Vector3 rightHandLastPosition;
    private Vector3 rightHandNewPosition;

    private Vector3 leftHandLastPosition;
    private Vector3 leftHandNewPosition;

    private bool isRightHand;
    private bool isLeftHand;

    [SerializeField] private float speed = 10000;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered - Rotate Around wheel");

        if (other.gameObject.name == rightHand.name && isLeftHand == false)
        {
            Debug.Log("isRightHand - Rotate Around wheel");
            isRightHand = true;
            rightHandLastPosition = rightHand.transform.position;
        }
        else if (other.gameObject.name == leftHand.name && isRightHand == false)
        {
            Debug.Log("isLeftHand - Rotate Around wheel");
            isLeftHand = true;
            leftHandLastPosition = leftHand.transform.position;
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (isRightHand)
        {
            rightHandNewPosition = rightHand.transform.position;
            float turnForce = rightHandLastPosition.x - rightHandNewPosition.x;
                        
            transform.RotateAround(transform.position, Vector3.forward, turnForce * speed * Time.deltaTime);

            rightHandLastPosition = rightHandNewPosition;
        }
        else if (isLeftHand)
        {
            leftHandNewPosition = leftHand.transform.position;
            float turnForce = leftHandLastPosition.x - leftHandNewPosition.x;
                        
            transform.RotateAround(transform.position, Vector3.forward, turnForce * speed * Time.deltaTime);

            leftHandLastPosition = leftHandNewPosition;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == rightHand.name) isRightHand = false;
        else if (other.gameObject.name == leftHand.name) isLeftHand = false;
    }

}
