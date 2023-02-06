using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wheel : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;
    // [SerializeField] private Transform centerOfRotationObject; // Must NOT be a child of the wheel.
    private Vector3 startPosition;
    private Rigidbody rb;

    private float speed = 15;

    private Vector3 rightHandLastPosition;
    private Vector3 rightHandNewPosition;

    private Vector3 leftHandLastPosition;
    private Vector3 leftHandNewPosition;

    private bool isRightHand;
    private bool isLeftHand;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == rightHand.name && isLeftHand == false)
        {
            Debug.Log("isRightHand - Addforce wheel");
            isRightHand = true;
            rightHandLastPosition = rightHand.transform.position;
        }
        else if (other.gameObject.name == leftHand.name && isRightHand == false)
        {
            Debug.Log("isLeftHand - Addforce wheel");
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

            rb.AddTorque(new Vector3(0, 0, turnForce) * speed);

            rightHandLastPosition = rightHandNewPosition;
        }
        else if (isLeftHand)
        {
            leftHandNewPosition = leftHand.transform.position;
            float turnForce = leftHandLastPosition.x - leftHandNewPosition.x;

            rb.AddTorque(new Vector3(0, 0, turnForce) * speed);

            leftHandLastPosition = leftHandNewPosition;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == rightHand.name) isRightHand = false;
        else if (other.gameObject.name == leftHand.name) isLeftHand = false;
    }
}
