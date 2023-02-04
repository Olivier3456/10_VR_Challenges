using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TestHaptic : XRBaseControllerInteractor
{
    private float _vibrationsIntensity = 1f;
    private float _vibrationsDuration = 0.2f;

    public void Vibration()
    {
        SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
    }
}
