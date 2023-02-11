using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelNeedle : MonoBehaviour
{
    public int graduation { get; private set; }

    [SerializeField] private ActionBasedController rightHandController;      // Needed to send vibrations to the controller.
    // [SerializeField] private ActionBasedController leftHandController;

    [SerializeField] private Transform wheel;

    [SerializeField] private TextMeshProUGUI numberDisplayTMP;
    private AudioSource audioSource;
    private WheelVibrations vibrations;

    [SerializeField] private int numberOfGraduations = 10;
    private float[] anglesOfGraduations;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        vibrations = GetComponent<WheelVibrations>();              

        anglesOfGraduations = new float[numberOfGraduations];
        float angleBetweenTwoClicks = 360 / numberOfGraduations;
        float angleOfLastClick = -angleBetweenTwoClicks;
        for (int i = 0; i < numberOfGraduations; i++)
        {
            angleOfLastClick += angleBetweenTwoClicks;
            if (angleOfLastClick > 180) angleOfLastClick = -(360 - angleOfLastClick);
            anglesOfGraduations[i] = angleOfLastClick;
            Debug.Log("Click " + i + ": angle of " + anglesOfGraduations[i] + "°.");
        }
    }


    public void VerifyGraduation(float wheelSpeed, float wheelAngle)
    {
        wheelSpeed = Mathf.Abs(wheelSpeed);
        Debug.Log("wheelSpeed: " + wheelSpeed + "; " + "wheelAngle: " + wheelAngle);

        for (int i = 0; i < anglesOfGraduations.Length; i++)
        {
            if (Mathf.Abs(anglesOfGraduations[i] - wheelAngle) < wheelSpeed * Time.deltaTime * 100)      // If actual angle of the wheel is near a click angle
            {
                numberDisplayTMP.text = i.ToString();
                if (!audioSource.isPlaying) audioSource.Play();
                vibrations.SendHapticImpulseToHand(rightHandController);
            }
        }      
    }





    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.name + " is in the needle trigger");
    //    graduation = int.Parse(other.name);
    //    numberDisplayTMP.text = graduation.ToString();
    //    audioSource.Play();
    //    vibrations.SendHapticImpulseToHand(rightHandController);
    //}
}
