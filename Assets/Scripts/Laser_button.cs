using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_button : MonoBehaviour
{    
    [SerializeField] private AudioSource _laserButtonAudioSource;
    private bool _timer = true;

    public bool LaserIsActive { get ; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hands") && _timer)
        {
            _timer = false;
            LaserIsActive = !LaserIsActive;
            _laserButtonAudioSource.Play();
            StartCoroutine(delayBeforeNextPush());
        }
    }

    IEnumerator delayBeforeNextPush()
    {
        yield return new WaitForSeconds(0.5f);
        _timer = true;
    }
}
