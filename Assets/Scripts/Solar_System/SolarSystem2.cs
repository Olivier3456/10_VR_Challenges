using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SolarSystem2 : MonoBehaviour     // Don't forget to set this attach point in the XR Grab Interactable too.
{
    [SerializeField] private Transform attachPoint;
    [Space(10)]
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;

    private Transform handGrabbing;

    [SerializeField] private InputActionReference resizeRightAction;
    [SerializeField] private InputActionReference resizeLeftAction;

    bool isRescaling;
    float originalDistance;
    Vector3 originalScale;

    bool rightHandInCollider;
    bool leftHandInCollider;

    private void Start()
    {
        resizeRightAction.action.Enable();
        resizeRightAction.action.performed += ResizeRight;

        resizeLeftAction.action.Enable();
        resizeLeftAction.action.performed += ResizeLeft;
    }


    private void Update()
    {
        if (isRescaling)
        {
            float handDistance = Vector3.Distance(rightHand.position, leftHand.position);
            transform.localScale = new Vector3(handDistance / originalDistance * originalScale.x,
                                               handDistance / originalDistance * originalScale.y,
                                               handDistance / originalDistance * originalScale.z);

            if (!rightHandInCollider || !leftHandInCollider) isRescaling = false;
        }
    }


    public void grabbed(SelectEnterEventArgs interactor)   // Must be called in the Select Entered event of the XR Grab Interactable.
    {
        handGrabbing = interactor.interactorObject.transform;

        if (handGrabbing.name == rightHand.name)
        {
            attachPoint.position = rightHand.position;
            attachPoint.rotation = rightHand.rotation;
        }
        else if (handGrabbing.name == leftHand.name)
        {
            attachPoint.position = leftHand.position;
            attachPoint.rotation = leftHand.rotation;
        }
    }

   

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " est dans le collider.");
        if (other.name == rightHand.name)
        {
            rightHandInCollider = true;           
        }
        else if (other.name == leftHand.name)
        {
            leftHandInCollider = true;            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + " n'est plus dans le collider.");
        if (other.name == rightHand.name)
        {
            rightHandInCollider = false;            
        }
        else if (other.name == leftHand.name)
        {
            leftHandInCollider = false;           
        }
    }



    public void ResizeRight(InputAction.CallbackContext obj)     // Ne pas oublier de mettre l'input en Press and Release.
    {
        if (rightHandInCollider && handGrabbing != null && handGrabbing.name == leftHand.name)
        {
            originalDistance = Vector3.Distance(rightHand.position, leftHand.position);
            isRescaling = !isRescaling;
            originalScale = transform.localScale;
        }        
    }

    public void ResizeLeft(InputAction.CallbackContext obj)     // Ne pas oublier de mettre l'input en Press and Release.
    {
        if (leftHandInCollider && handGrabbing != null && handGrabbing.name == rightHand.name)
        {
            originalDistance = Vector3.Distance(rightHand.position, leftHand.position);
            isRescaling = !isRescaling;
            originalScale = transform.localScale;
        }
    }
}
