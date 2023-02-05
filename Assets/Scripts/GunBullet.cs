using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunBullet : MonoBehaviour
{
    private AudioSource _audioSource;
    private bool _soundPlayed = false;
    [SerializeField] private AudioClip[] _bulletFallingSounds;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_soundPlayed)
        {
            _soundPlayed = true;
            _audioSource.clip = _bulletFallingSounds[Random.Range(0, _bulletFallingSounds.Length)];
            _audioSource.Play();
        }
    }
}
