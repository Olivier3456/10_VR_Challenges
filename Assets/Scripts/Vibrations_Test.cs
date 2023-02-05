using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.XR.Interaction.Toolkit;


// Attach this script to the object that send vibrations to the hand which grabbed it.


public class Vibrations_Test : MonoBehaviour
{
    [SerializeField] private ActionBasedController rightHandController;
    [SerializeField] private ActionBasedController leftHandController;
    [Space(10)]
    [SerializeField] private float _vibrationsIntensity = 1f;
    [SerializeField] private float _vibrationsDuration = 0.2f;

    private bool grabbedByLeftHand;
    
    private XRBaseInteractor _handGrabbingGun;
    
    public void SelectEntered(SelectEnterEventArgs interactor)  // Methode to call when the object is grabbed (selected).
    {
        _handGrabbingGun = interactor.interactorObject as XRBaseInteractor;        
    }

    public void SendHapticImpulse()     // Send vibrations to the hand which grabbed it.
    {
        if (_handGrabbingGun != null && _handGrabbingGun.gameObject.name == "LeftHand Controller") grabbedByLeftHand = true;
        else grabbedByLeftHand = false;

        if (!grabbedByLeftHand)
            rightHandController.SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
        else
            leftHandController.SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
    }
}