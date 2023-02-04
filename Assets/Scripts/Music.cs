using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private bool isPlayed;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayIfNotAlreadyPlayed()
    {
        if (!isPlayed)
        {
            isPlayed = true;
            audioSource.Play();
        }
    }
}
