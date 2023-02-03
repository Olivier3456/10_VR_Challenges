using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private AudioSource _gunAudioSource;
    [SerializeField] private Transform _endGunBarrel;
    [SerializeField] private ParticleSystem _impactParticle;
    [SerializeField] private ParticleSystem _gunFireParticle;
    [SerializeField] private float _gunForce = 1f;

    private Vector3 impactPosition;
    
    public void Fire()
    {
        // Shot only if the gun fire is over.
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Gun_Idle"))
        {
            _anim.SetTrigger("Shot");
            _gunAudioSource.Play();

            ParticleSystem gFParticle = Instantiate(_gunFireParticle, _endGunBarrel.position, Quaternion.identity);
            gFParticle.transform.forward = _endGunBarrel.forward;
            gFParticle.Play();
            


            
            Ray ray = new Ray(_endGunBarrel.position, _endGunBarrel.forward); // (Le raycast part du bout du canon.)
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                impactPosition = hit.point;
                Debug.Log(impactPosition);
                
                ParticleSystem sparks = Instantiate(_impactParticle, impactPosition, Quaternion.identity);
                sparks.transform.forward = hit.normal;   // (Oriente les particules à angle droit par rapport à la surface d'impact.)
                sparks.Play();



                if (hit.transform.gameObject.CompareTag("Canette"))
                {
                   hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(-hit.normal * _gunForce, ForceMode.Impulse);
                }
            }
        }
    }
}
