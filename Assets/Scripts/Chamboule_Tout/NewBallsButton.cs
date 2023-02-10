using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallsButton : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hands"))
        {
            gameManager.DestroyBalls();
            gameManager.InstantiateBalls();
            transform.gameObject.SetActive(false);
        }
    }
}
