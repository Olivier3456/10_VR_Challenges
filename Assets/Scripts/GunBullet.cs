using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunBullet : MonoBehaviour
{
    private AudioSource _audioSource;    
    private Rigidbody _rb;    
    private bool _grounded;
    [SerializeField] private AudioClip[] _bulletFallingSounds;    
    [SerializeField] private float _ejectionForce = 0.2f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {   
        if (_grounded)
        {
            StartCoroutine(WaitBeforeDeactivateRB());        // Déactivate rigidbody for avoiding the bullet moving endlessly on the ground.
        }        
    }


    void OnEnable()
    {        
        _grounded = false;        
        _rb.isKinematic = false;
        _rb.AddForce(-transform.forward * _ejectionForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_grounded)
        {           
            _grounded = true;
            _audioSource.clip = _bulletFallingSounds[Random.Range(0, _bulletFallingSounds.Length)];
            _audioSource.Play();
        }
    }


    IEnumerator WaitBeforeDeactivateRB()
    {
        yield return new WaitForSeconds(0.7f);
        _rb.isKinematic = true;
    }
}
