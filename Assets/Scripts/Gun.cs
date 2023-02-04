using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private AudioSource _gunAudioSource;
    [SerializeField] private AudioClip _gunShotSound;
    [SerializeField] private AudioClip _gunEmptySound;
    [SerializeField] private AudioClip _gunReloadSound;
    [SerializeField] private Transform _endGunBarrel;
    [SerializeField] private ParticleSystem _impactParticle;
    [SerializeField] private ParticleSystem _gunFireParticle;
    [SerializeField] private Light _FireGunPointLight;
    [SerializeField] private float _gunForce = 1f;
    [SerializeField] private float _reloadTime = 1.5f;
    [SerializeField] private int _magazineCapacity = 7;
    private int _bulletsLeft;
    private bool _isReloading;
    private float _verticalMarginForReloading = 0.025f;
    private Vector3 impactPosition;

    private void Start()
    {
        _bulletsLeft = _magazineCapacity;
        Debug.Log(Vector3.up);
    }

    private void Update()
    {
        // Reloading when the gun points downwards.
        if (!_gunAudioSource.isPlaying && _bulletsLeft < 7 && Vector3.Dot(-_endGunBarrel.forward, Physics.gravity.normalized) < -1 + _verticalMarginForReloading)
        {
            _bulletsLeft = 7;
            _gunAudioSource.clip = _gunReloadSound;
            _gunAudioSource.Play();
            _isReloading = true;
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(_reloadTime);
        _isReloading = false;
    }

    public void Fire()
    {
        // Shot only if the gun fire is over.
        if (!_isReloading && _anim.GetCurrentAnimatorStateInfo(0).IsName("Gun_Idle"))
        {
            if (_bulletsLeft > 0)
            {
                _bulletsLeft--;
                _anim.SetTrigger("Shot");
                _gunAudioSource.pitch = Random.Range(0.9f, 1.1f);
                _gunAudioSource.clip = _gunShotSound;
                _gunAudioSource.Play();
                _FireGunPointLight.gameObject.SetActive(true);
                StartCoroutine(WaitBeforeShutDownLight());

                GunSceneGameManager.Instance.ShotFired();

                _gunFireParticle.transform.position = _endGunBarrel.position;
                _gunFireParticle.transform.forward = _endGunBarrel.forward;
                _gunFireParticle.Play();




                Ray ray = new Ray(_endGunBarrel.position, _endGunBarrel.forward); // (Le raycast part du bout du canon.)
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    impactPosition = hit.point;
                    _impactParticle.transform.position = impactPosition;
                    _impactParticle.transform.forward = hit.normal;               // (Oriente les particules à angle droit par rapport à la surface d'impact.)
                    _impactParticle.Play();



                    if (hit.transform.gameObject.CompareTag("Canette"))
                    {
                        hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(-hit.normal * _gunForce, ForceMode.Impulse);

                        AudioSource canetteAudioSource = hit.transform.gameObject.GetComponent<AudioSource>();
                        canetteAudioSource.pitch = Random.Range(0.7f, 1.3f);
                        canetteAudioSource.Play();
                        GunSceneGameManager.Instance.OnePointMore();
                    }
                }
            }
            else
            {
                _gunAudioSource.clip = _gunEmptySound;
                _gunAudioSource.Play();
            }
        }
    }

    IEnumerator WaitBeforeShutDownLight()
    {
        yield return new WaitForSeconds(0.1f);
        _FireGunPointLight.gameObject.SetActive(false);
    }
}
