using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(WheelVibrations))]
public class WheelNeedle : MonoBehaviour
{
    public int graduation { get; private set; }

    [Tooltip("Needed to send vibrations to the controller.")]
    [SerializeField] private ActionBasedController rightHandController;
    // [SerializeField] private ActionBasedController leftHandController;

    [SerializeField] private Transform wheel;

    [SerializeField] private TextMeshProUGUI numberDisplayTMP;
    private AudioSource audioSource;
    private WheelVibrations vibrations;

    private WheelGameManager wheelGameManager;

    [SerializeField] private int numberOfGraduations = 10;
    private float[] anglesOfGraduations;
    public int graduationNumber { get; private set; }

    private int lastClick = 0;

    private void Start()
    {
        graduationNumber = 0;

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
            //    Debug.Log("Click " + i + ": angle of " + anglesOfGraduations[i] + "°.");

            wheelGameManager = GameObject.Find("Game Manager").GetComponent<WheelGameManager>();
        }
    }


    public void VerifyGraduation(float wheelSpeed, float wheelAngle)
    {
        wheelSpeed = Mathf.Abs(wheelSpeed);
        //   Debug.Log("wheelSpeed: " + wheelSpeed + "; " + "wheelAngle: " + wheelAngle);

        for (int i = 0; i < anglesOfGraduations.Length; i++)
        {
            if (lastClick != i) //  To avoid the wheel to do several clicks at the same location.
            {
                if (Mathf.Abs(anglesOfGraduations[i] - wheelAngle) < wheelSpeed * Time.deltaTime * 100)      // If the actual angle of the wheel is near a click angle. Last number may be adjusted in proportion of the clicks distance.
                {
                    lastClick = i;
                    numberDisplayTMP.text = i.ToString();
                    if (!audioSource.isPlaying) audioSource.Play();
                    vibrations.SendHapticImpulseToHand(rightHandController);
                    graduationNumber = i;
                    wheelGameManager.UpdateCode(i);
                }
            }
        }
    }
}
