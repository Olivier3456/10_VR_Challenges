using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Planet : MonoBehaviour     // Don't forget to set this attach point in the XR Grab Interactable too.
{
    [SerializeField] private Transform attachPoint;
    [Space(10)]
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [Space(10)]
    [SerializeField] private Transform sun;

    private Transform handGrabbing;

    [SerializeField] private InputActionReference resizeRightAction;
    [SerializeField] private InputActionReference resizeLeftAction;

    bool isRescaling;
    float originalDistance;
    Vector3 originalScale;

    bool rightHandInCollider;
    bool rightResizeTriggerPressed;

    bool leftHandInCollider;
    bool leftResizeTriggerPressed;


    private AudioSource audioSource;

    private void Start()
    {
        resizeRightAction.action.Enable();
        resizeRightAction.action.performed += ResizeRight;

        resizeLeftAction.action.Enable();
        resizeLeftAction.action.performed += ResizeLeft;

        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (isRescaling)
        {
            float handDistance = Vector3.Distance(rightHand.position, leftHand.position);
            transform.localScale = new Vector3(handDistance / originalDistance * originalScale.x,
                                               handDistance / originalDistance * originalScale.y,
                                               handDistance / originalDistance * originalScale.z);

            audioSource.pitch = originalDistance / handDistance;
            if (!audioSource.isPlaying) audioSource.Play();
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


    public void unGrabbed(SelectExitEventArgs interactor)
    {
        transform.LookAt(sun.position);
    }



    private void OnTriggerEnter(Collider other)
    {
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
        if (other.name == rightHand.name)
        {
            rightHandInCollider = false;
            //    isRescaling = false;   // Uncomment if you wan't the rescale stops when the hand exits the object.
        }
        else if (other.name == leftHand.name)
        {
            leftHandInCollider = false;
            //    isRescaling = false;    // Uncomment if you wan't the rescale stops when the hand exits the object.
        }
    }



    public void ResizeRight(InputAction.CallbackContext obj)     // Ne pas oublier de mettre l'input en Press and Release.
    {
        rightResizeTriggerPressed = !rightResizeTriggerPressed;

        if (rightResizeTriggerPressed && rightHandInCollider && handGrabbing != null && handGrabbing.name == leftHand.name)
        {
            originalDistance = Vector3.Distance(rightHand.position, leftHand.position);
            originalScale = transform.localScale;
            isRescaling = true;
        }
        if (!rightResizeTriggerPressed && !leftResizeTriggerPressed)
        {
            isRescaling = false;
            audioSource.Stop();
        }
    }

    public void ResizeLeft(InputAction.CallbackContext obj)     // Ne pas oublier de mettre l'input en Press and Release.
    {
        leftResizeTriggerPressed = !leftResizeTriggerPressed;

        if (leftResizeTriggerPressed && leftHandInCollider && handGrabbing != null && handGrabbing.name == rightHand.name)
        {
            originalDistance = Vector3.Distance(rightHand.position, leftHand.position);
            originalScale = transform.localScale;
            isRescaling = true;
        }
        if (!rightResizeTriggerPressed && !leftResizeTriggerPressed)
        {
            isRescaling = false;
            audioSource.Stop();
        }
    }
}
