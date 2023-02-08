using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class RotateAroundProjection : MonoBehaviour
{
    [SerializeField] private InputActionReference _gripWithRightHand;       // Grip action réglée en Press > Press Only.
    [SerializeField] private InputActionReference _unGripWithRightHand;     // Grip action sur la même gâchette, réglée en Press > Release Only.
    private bool _isGrippedByRightHand;


    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    private Vector3 rightHandLastPosition;
    private Vector3 rightHandNewPosition;

    private Vector3 leftHandLastPosition;
    private Vector3 leftHandNewPosition;

    private int rightHandInColliders = 0;       // La roue a deux colliders, la main ne pourra la tourner que lorsqu'elle sera dans les deux colliders à la fois.
    private int leftHandInColliders = 0;

    [SerializeField] private float speed = 50000;

    private float wheelRadius;
    private float wheelPerimeter;

    private AudioSource _wheelAudioSource;
    private Vector3 lastRotationClick;
    private bool alreadyClickedOnReturn;
    [SerializeField] private float _angleBetweenClicks = 5;



    [SerializeField] private GameObject ObjectToProjectJustForTest;
    [SerializeField] private GameObject parentObject;
    private Vector3 pointToProject;
    private Vector3 wheelNormal;
    private Vector3 planePoint;





    void Update()
    {        
        ProjectHandPositionToWheelPlan();
    }

    private void ProjectHandPositionToWheelPlan()
    {
        wheelNormal = parentObject.transform.forward;
        pointToProject = rightHand.transform.position;
        planePoint = transform.position;
        Vector3 projection = pointToProject - (Vector3.Dot(wheelNormal, pointToProject - planePoint) / Vector3.Dot(wheelNormal, wheelNormal)) * wheelNormal;
        ObjectToProjectJustForTest.transform.position = projection;
    }

    private void Start()
    {
        wheelRadius = transform.localScale.x * 0.5f;
        wheelPerimeter = wheelRadius * 2 * 3.14159f;
        Debug.Log("Wheel radius: " + wheelRadius + "; wheel perimeter: " + wheelPerimeter);


        _wheelAudioSource = GetComponent<AudioSource>();
        lastRotationClick = transform.forward;
        _angleBetweenClicks /= 90;


        _gripWithRightHand.action.Enable();
        _gripWithRightHand.action.performed += GripTheWheel;
        _unGripWithRightHand.action.Enable();
        _unGripWithRightHand.action.performed += UnGripTheWheel;
    }


    private void GripTheWheel(InputAction.CallbackContext obj)
    {
        _isGrippedByRightHand = true;
        rightHandLastPosition = rightHand.transform.position;
    }
    private void UnGripTheWheel(InputAction.CallbackContext obj)
    {
        _isGrippedByRightHand = false;
    }




    private void OnTriggerEnter(Collider other)     // Méthode appelée chaque fois que quelque chose entre dans un des deux colliders.
    {
        if (other.name == rightHand.name)
        {
            rightHandInColliders++;
            if (rightHandInColliders == 2)
                rightHandLastPosition = rightHand.transform.position;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        float distanceHandToAxe = Vector3.Distance(rightHand.transform.position, transform.position);

        // La main droite peut faire bouger la roue si elle est dans les deux colliders, si le bouton du grip est pressée, et si elle est à peu près située sur le tour de la roue.
        if (rightHandInColliders == 2 && _isGrippedByRightHand && distanceHandToAxe > wheelRadius - 0.09f && distanceHandToAxe < wheelRadius + 0.01f)
        {
            rightHandNewPosition = rightHand.transform.position;

            // La roue va tourner autour de son propre axe, spécifié comme étant local (transform.TransformDirection) :
            transform.RotateAround(transform.position, transform.TransformDirection(Vector3.up), TurnForce(rightHandNewPosition, rightHandLastPosition) * speed * Time.deltaTime);

            rightHandLastPosition = rightHandNewPosition;
            CheckForClick();
        }


        //else if (leftHandInColliders == 2)
        //{
        //    leftHandNewPosition = leftHand.transform.position;
        //    float turnForce = leftHandLastPosition.x - leftHandNewPosition.x;

        //    transform.RotateAround(transform.position, Vector3.forward, TurnForce(leftHandNewPosition,leftHandLastPosition) * speed * Time.deltaTime);

        //    leftHandLastPosition = leftHandNewPosition;
        //}
    }

    private void CheckForClick()
    {
        float angleFromLastClick = Vector3.Dot(transform.forward, lastRotationClick.normalized);
        if (angleFromLastClick < 1 - _angleBetweenClicks)       // Quand la roue arrive au prochain click :
        {
            _wheelAudioSource.Play();
            lastRotationClick = transform.forward;
        }

        if (angleFromLastClick > 0.999f && !alreadyClickedOnReturn) // Quand la roue repasse dans la zone de son dernier click et n'a pas déjà cliqué lors de ce retour.
        {
            _wheelAudioSource.Play();
            alreadyClickedOnReturn = true;
        }
        else if (angleFromLastClick <= 0.999f) alreadyClickedOnReturn = false;       // Si elle sort de la zone du dernier click, elle pourra y recliquer.
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name == rightHand.name)
        {
            rightHandInColliders--;
        }
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

        float xFactor = positiveHandPositionX / wheelPerimeter;        // Pour que nos variables dépendent de la taille de la roue.
        float yFactor = positiveHandPositionY / wheelPerimeter;


        float handMovementX = lastPositionOfHand.x - newPositionOfHand.x;   // Distance parcourue par la main depuis la dernière image.
        float handMovementY = lastPositionOfHand.y - newPositionOfHand.y;


        float turnForce;
        if (xFactor < 0 && yFactor > 0 || xFactor > 0 && yFactor < 0) turnForce = -(handMovementX * yFactor - handMovementY * xFactor);
        else turnForce = handMovementX * yFactor - handMovementY * xFactor;




        //float axeToHandDistanceFactor = 1 / (Vector3.Distance(newPositionOfHand, transform.position) / wheelRadius);  // Plus la main est proche de l'axe, plus la roue va tourner vite pour un même mouvement de main.
        //if (axeToHandDistanceFactor < 1) axeToHandDistanceFactor = 1;
        //else if (axeToHandDistanceFactor > 10) axeToHandDistanceFactor = 10;    // On va le limiter à 10 pour que la roue ne tourne pas trop vite quand la main est très proche de son axe.

        //turnForce *= axeToHandDistanceFactor;
        //Debug.Log("Distance de la main au centre de la roue : " + (Vector3.Distance(newPositionOfHand, transform.position) + "Divisé par le rayon de la roue : " + (Vector3.Distance(newPositionOfHand, transform.position) / wheelRadius) + " . axeToHandDistanceFactor : " + axeToHandDistanceFactor));

        return turnForce;
    }
}
