using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource _gunAudioSource;


   

    public void Fire()
    {       
        // Shot only if the gun fire is over.
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Gun_Idle"))
        {
            anim.SetTrigger("Shot");
            _gunAudioSource.Play();
        }
    }
}
