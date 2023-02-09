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

    private void Start()
    {
        resizeRightAction.action.Enable();
        resizeRightAction.action.performed += Resize;
        originalScale = transform.localScale;
    }


    private void Update()
    {
        if (isRescaling)
        {
            float handDistance = Vector3.Distance(rightHand.position, leftHand.position);
            transform.localScale = new Vector3(handDistance / originalDistance * originalScale.x,
                                               handDistance / originalDistance * originalScale.y,
                                               handDistance / originalDistance * originalScale.z);
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


    public void Resize(InputAction.CallbackContext context)
    {
        originalDistance = Vector3.Distance(rightHand.position, leftHand.position);
        isRescaling = true;
        Debug.Log("Resize");
    }
}
