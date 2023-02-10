using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WheelNeedle : MonoBehaviour
{
    public int graduation { get; private set; }

    [SerializeField] private ActionBasedController rightHandController;      // Sert pour envoyer les vibrations aux manettes quand la roue tourne.
    //  [SerializeField] private ActionBasedController leftHandController;

    [SerializeField] private TextMeshProUGUI numberDisplayTMP;
    private AudioSource audioSource;
    private WheelVibrations vibrations;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        vibrations = GetComponent<WheelVibrations>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " is in the needle trigger");
        graduation = int.Parse(other.name);
        numberDisplayTMP.text = graduation.ToString();
        audioSource.Play();
        vibrations.SendHapticImpulseToHand(rightHandController);
    }
}
