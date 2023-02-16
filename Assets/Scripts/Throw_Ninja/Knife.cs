using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DisplayUseGravity());
    }


    IEnumerator DisplayUseGravity()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("rb.useGravity = " + rb.useGravity);
        StartCoroutine(DisplayUseGravity());
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plantable"))
        {
            Debug.Log("OnCollisionEnter() with a Plantable object : useGravity set to false.");
            StayImmobile();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plantable")) EnableRbGravity();
    }



    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Plantable"))
    //    {
    //        rb.useGravity = true;

    //        Debug.Log("OnTriggerExit(): rb.useGravity: " + rb.useGravity);
    //    }
    //}




    private void StayImmobile()
    {
        rb.useGravity = false;
           //rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void EnableRbGravity()
    {
        rb.useGravity = true;

           //rb.isKinematic = false;
        Debug.Log("EnableRbGravity(): rb.useGravity: " + rb.useGravity);
    }
}
