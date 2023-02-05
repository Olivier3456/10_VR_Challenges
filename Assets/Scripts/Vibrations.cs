using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.XR.Interaction.Toolkit;


// Attach this script to the object that send vibrations to the hand which grabbed it.
// Contains a public method to send vibrations when grabbed (selected),
// and a second public method to send vibrations when activated.


public class Vibrations : MonoBehaviour
{
    [SerializeField] private ActionBasedController rightHandController;
    [SerializeField] private ActionBasedController leftHandController;
    [Space(10)]
    [SerializeField] private float _vibrationsIntensity = 1f;
    [SerializeField] private float _vibrationsDuration = 0.2f;

    private bool grabbedByLeftHand;

    private XRGrabInteractable _grabInteractable;
    private XRBaseInteractor _handGrabbingGun;

    private void OnEnable()
    {
        _grabInteractable.selectEntered.AddListener(SelectEntered);
        _grabInteractable.selectExited.AddListener(SelectExited);
    }


    private void OnDisable()
    {
        _grabInteractable.selectEntered.RemoveListener(SelectEntered);
        _grabInteractable.selectExited.RemoveListener(SelectExited);
    }


    private void SelectEntered(SelectEnterEventArgs interactor)
    {
        _handGrabbingGun = interactor.interactorObject as XRBaseInteractor;
    }


    private void SelectExited(SelectExitEventArgs interactor)
    {
        _handGrabbingGun = null;
    }


    private void Awake()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }


    public void VibrateWhenActivated()
    {
        SendHapticImpulseToHand();
    }


    public void VibrateWhenSelected()
    {
        StartCoroutine(WaitForHandCheckBeforeSendHapticImpulse());
    }
    IEnumerator WaitForHandCheckBeforeSendHapticImpulse()
    {
        yield return null;  // Wait for next frame, giving enough time to SelectEntered() to be executed before sending haptic impulse to the hand.
                            // Necessary in the case where the object send vibrations when grabbed (selected).
        SendHapticImpulseToHand();
    }


    private void SendHapticImpulseToHand()
    {
        if (_handGrabbingGun != null && _handGrabbingGun.gameObject.name == "LeftHand Controller") grabbedByLeftHand = true;
        else grabbedByLeftHand = false;

        if (!grabbedByLeftHand)
            rightHandController.SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
        else
            leftHandController.SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
    }
}
