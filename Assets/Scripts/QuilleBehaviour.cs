using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuilleBehaviour : MonoBehaviour
{
    private GameManager gameManager;

    private float hauteurDeDepart;

    [SerializeField] AudioSource quilleAudioSource;   

    private bool quilleTombee;
    void Start()
    {
        hauteurDeDepart = transform.position.y;
        gameManager= GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    
    void Update()
    {
        if (transform.position.y < hauteurDeDepart - 0.05f && !quilleTombee)
        {
            gameManager.QuilleTombee();
            quilleTombee = true;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {       
            quilleAudioSource.Play();        
    }
}
