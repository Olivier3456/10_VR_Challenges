using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelVibrations : MonoBehaviour
{     
    [SerializeField][Range(0, 1)] private float _vibrationsIntensity = 0.1f;
    [SerializeField] private float _vibrationsDuration = 0.1f;

    public void SendHapticImpulseToHand(ActionBasedController hand)
    {        
            hand.SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);     
    }
}
