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
    public int lastClick { get; private set; }
    

    private void Start()
    {
        lastClick = 0;

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

            wheelGameManager = GameObject.Find("Game Manager").GetComponent<WheelGameManager>();
        }
    }


    public void VerifyGraduation(float wheelSpeed, float wheelAngle)
    {
        wheelSpeed = Mathf.Abs(wheelSpeed);

        if (wheelSpeed < 100 / numberOfGraduations)     // First case: the wheel turns slowly. Precise detection of clicks, but can miss clicks if the wheel turns too fast.
        {    
            for (int i = 0; i < anglesOfGraduations.Length; i++)
            {
                if (lastClick != i) //  To avoid the wheel to do several clicks at the same location.
                {
                    if (Mathf.Abs(anglesOfGraduations[i] - wheelAngle) < wheelSpeed)
                    {
                        lastClick = i;
                        numberDisplayTMP.text = i.ToString();
                        if (!audioSource.isPlaying) audioSource.Play();
                        vibrations.SendHapticImpulseToHand(rightHandController);                        
                        wheelGameManager.UpdateCode(i);
                    }
                }
            }
        }

        else  // Second case: wheel turns faster. Less precise detection of clicks, but haven't missed any clicks during my tests.
        {           
            float angleFromWheelToNearestClick = 361;
            int nearestClick = lastClick;

            for (int i = 0; i < anglesOfGraduations.Length; i++)
            {
                float angleFromWheelToClickAtIndexI = Mathf.Abs(anglesOfGraduations[i] - wheelAngle);
              
                if (angleFromWheelToNearestClick > angleFromWheelToClickAtIndexI)
                {
                    angleFromWheelToNearestClick = angleFromWheelToClickAtIndexI;
                    nearestClick = i;                   
                }
            }
           
            if (nearestClick != lastClick)
            {
                numberDisplayTMP.text = nearestClick.ToString();
                if (!audioSource.isPlaying) audioSource.Play();
                vibrations.SendHapticImpulseToHand(rightHandController);                
                wheelGameManager.UpdateCode(nearestClick);            
            }

            lastClick = nearestClick;
        }
    }
}
