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

    [SerializeField] private float speed = 51000;

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

            // La roue va tourner autour de son propre axe :
            transform.RotateAround(transform.position, Vector3.forward, TurnForce(rightHandNewPosition, rightHandLastPosition) * speed * Time.deltaTime);

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
        float handPositionX = newPositionOfHand.x - transform.position.x;
        float handPositionY = newPositionOfHand.y - transform.position.y;

        float positiveHandPositionX = Mathf.Abs(handPositionX);   // Pour que le mouvement ne s'inverse pas quand la main passe à gauche du centre de la roue
                                                                  // (mais on doit conserver le signe de la variable pour le calcul suivant).

        float positiveHandPositionY = Mathf.Abs(handPositionY);   // Pour que le mouvement ne s'inverse pas quand la main passe en-dessous du centre de la roue.


        if (handPositionY < 0) { positiveHandPositionX = -positiveHandPositionX; }  // Pour que le mouvement s'inverse quand on tourne la roue dans sa moitié basse.       
        if (handPositionX < 0) { positiveHandPositionY = -positiveHandPositionY; }  // Pour que le mouvement s'inverse quand on tourne la roue dans sa moitié gauche.

        float xFactor = positiveHandPositionX / wheelPerimeter;        // Etape finale de ce calcul, pour que nos variables dépendent de la taille de la roue.
        float yFactor = positiveHandPositionY / wheelPerimeter;


        float handMovementX = lastPositionOfHand.x - newPositionOfHand.x;   // Distance parcourue par la main depuis la dernière image.
        float handMovementY = lastPositionOfHand.y - newPositionOfHand.y;

        Debug.Log("xFactor : " + xFactor);
        Debug.Log("yFactor : " + yFactor);


        float turnForce;

        turnForce = handMovementX * yFactor - handMovementY * xFactor;






        return turnForce;
    }

}
