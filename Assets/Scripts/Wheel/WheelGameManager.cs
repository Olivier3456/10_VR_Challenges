using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WheelGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI codeText;

    private int actualGraduation;
    private int lastGraduation = 0;
    private int penultimateGraduation = 0;

    private AudioSource audioSource;
    [SerializeField] AudioClip yesSound;
    [SerializeField] AudioClip noSound;


    private string code = "";

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void UpdateCode(int graduation)
    {
        penultimateGraduation = lastGraduation;
        lastGraduation = actualGraduation;
        actualGraduation = graduation;

        if (code == "9472" && graduation == 8)
        {
            codeText.text = "94728";
            audioSource.clip = yesSound;
            audioSource.Play();
        }

        if (actualGraduation == penultimateGraduation)   // If the wheel has gone backwards
        {
            code += lastGraduation.ToString();
            codeText.text = code;
            if (code.Length == 5 && code != "94728")
            {
                audioSource.clip = noSound;
                audioSource.Play();
                code = "";
                codeText.text = code;
            }
        }
    }
}
