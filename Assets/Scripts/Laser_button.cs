using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_button : MonoBehaviour
{    
    [SerializeField] private AudioSource _laserButtonAudioSource;

    public bool LaserIsActive { get ; private set; }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hands"))
        {
            LaserIsActive = !LaserIsActive;
        }
    }
}
