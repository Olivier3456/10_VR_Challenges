using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystemSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private Transform parent;
    [SerializeField] private Rigidbody parentRigidbody;
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hands"))
        {
            audioSource.volume = parentRigidbody.velocity.magnitude;           
            audioSource.pitch = 1.5f / parent.localScale.x;
            audioSource.Play();
        }
    }
}
