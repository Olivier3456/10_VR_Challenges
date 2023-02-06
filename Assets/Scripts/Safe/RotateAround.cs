using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] private float speed = 5000;

    private float wheelRadius;
    private float wheelPerimeter;

    private void Start()
    {
        wheelRadius = transform.localScale.x * 0.5f;
        wheelPerimeter = wheelRadius * 2 * 3.14159f;
        Debug.Log("Wheel radius: " + wheelRadius + "; wheel perimeter: " + wheelPerimeter);

        // distanceHandAxe = Vector3.Distance(rightHand.transform.position, transform.position);        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.name == rightHand.name && isLeftHand == false)
        {
            isRightHand = true;
            rightHandLastPosition = rightHand.transform.position;
        }
        else if (other.name == leftHand.name && isRightHand == false)
        {
            isLeftHand = true;
            leftHandLastPosition = leftHand.transform.position;
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (isRightHand)
        {
            rightHandNewPosition = rightHand.transform.position;
         //   float turnForce = rightHandLastPosition.x - rightHandNewPosition.x;

            transform.RotateAround(transform.position, Vector3.forward, TurnForce(rightHandNewPosition, rightHandLastPosition)
                * wheelPerimeter * speed * Time.deltaTime);

            rightHandLastPosition = rightHandNewPosition;
        }
        //else if (isLeftHand)
        //{
        //    leftHandNewPosition = leftHand.transform.position;
        //    float turnForce = leftHandLastPosition.x - leftHandNewPosition.x;

        //    transform.RotateAround(transform.position, Vector3.forward, turnForce * wheelPerimeter * speed * Time.deltaTime);

        //    leftHandLastPosition = leftHandNewPosition;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == rightHand.name) isRightHand = false;
        else if (other.gameObject.name == leftHand.name) isLeftHand = false;
    }


    private float TurnForce(Vector3 newPositionOfHand, Vector3 lastPositionOfHand)
    {
        // Position de la main par rapport au centre de la roue :
      //  Vector3 handPosition = newPositionOfHand - transform.position;

        float handPositionX = newPositionOfHand.x - transform.position.x;
        float handPositionY = newPositionOfHand.y - transform.position.y;
        if (handPositionX < 0) { handPositionX = - handPositionX; } // On ne veut pas de valeurs négatifs, juste une distance.
        if (handPositionY < 0) { handPositionX = -handPositionX; }  // Pour que le mouvement s'inverse si on tourne la roue dans sa moitié basse.



        float handMovementX = lastPositionOfHand.x - newPositionOfHand.x;
        float handMovementY = lastPositionOfHand.y - newPositionOfHand.y;
        float handMovementZ = lastPositionOfHand.z - newPositionOfHand.z;

        float turnForce = handMovementX / handPositionX;

        return turnForce;
    }

}
