using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VibrationsController : MonoBehaviour
{
    [SerializeField] private ActionBasedController handController;
    [Space(10)]
    [SerializeField][Range(0, 1)] private float _vibrationsIntensity = 1f;
    [SerializeField] private float _vibrationsDuration = 0.1f;

    [SerializeField] private float vibrationDelay = 0.4f;
    private bool vibrationDelayEnded = true;


    public void SendHapticImpulse()     // Send vibrations to the hand which grabbed it.
    {
        if (vibrationDelayEnded)
        {
            vibrationDelayEnded = false;
            handController.SendHapticImpulse(_vibrationsIntensity, _vibrationsDuration);
            StartCoroutine(VibrationDelay());
        }
    }


    private IEnumerator VibrationDelay()
    {
        yield return new WaitForSeconds(vibrationDelay);
        vibrationDelayEnded = true;
    }
}
