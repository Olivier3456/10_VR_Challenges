using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.XR.Interaction.Toolkit;


// Attach this script to the object that sends vibrations to the hand which grabbed it.


public class Vibrations_Test : MonoBehaviour
{
    [SerializeField] private ActionBasedController rightHandController;
    [SerializeField] private ActionBasedController leftHandController;
    [Space(10)]
    [SerializeField] private float _vibrationsIntensity = 1f;
    [SerializeField] private float _vibrationsDuration = 0.2f;

    private bool grabbedByLeftHand;
    
    private XRBaseInteractor _handGrabbingGun;


    // This method allows us to know which hand has grabbed the object.
    // There is an overload for each kind of possible event:
    #region
    public void SendHapticImpulse(HoverEnterEventArgs interactor)
    {
        _handGrabbingGun = interactor.interactorObject as XRBaseInteractor;
        SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
    }
    public void SendHapticImpulse(HoverExitEventArgs interactor)
    {
        _handGrabbingGun = interactor.interactorObject as XRBaseInteractor;
        SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
    }
    public void SendHapticImpulse(SelectEnterEventArgs interactor)
    {
        _handGrabbingGun = interactor.interactorObject as XRBaseInteractor;
        SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
    }
    public void SendHapticImpulse(SelectExitEventArgs interactor)
    {
        _handGrabbingGun = interactor.interactorObject as XRBaseInteractor;
        SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
    }
    public void SendHapticImpulse(ActivateEventArgs interactor)
    {
        _handGrabbingGun = interactor.interactorObject as XRBaseInteractor;
        SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
    }
    public void SendHapticImpulse(DeactivateEventArgs interactor)
    {
        _handGrabbingGun = interactor.interactorObject as XRBaseInteractor;
        SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
    }
    #endregion

       

    // Send vibrations to the hand which grabbed it. (Can of course be called by another class.)
    public void SendHapticImpulse(float intensity, float duration)
    {
        if (_handGrabbingGun != null && _handGrabbingGun.gameObject.name == leftHandController.gameObject.name) grabbedByLeftHand = true;
        else grabbedByLeftHand = false;

        if (!grabbedByLeftHand)
            rightHandController.SendHapticImpulse(intensity, duration);
        else
            leftHandController.SendHapticImpulse(intensity, duration);
    }
}