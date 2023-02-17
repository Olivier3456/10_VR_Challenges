using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Knife_Throw : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;

    [SerializeField] private float forceMultiplier = 1;   

    public void ThrowKnife(SelectExitEventArgs interactor)  // DON'T FORGET TO CALL THIS METHOD WHEN SELECT EXITED!!
    {
    
        rigidBody.AddForce(rigidBody.velocity * forceMultiplier, ForceMode.Impulse);
    
    }


    
}
