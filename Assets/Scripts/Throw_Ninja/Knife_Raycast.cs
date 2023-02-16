using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_Raycast : MonoBehaviour
{
    [SerializeField] private float maxDistToPlant = 0.05f;
    [SerializeField] private float minKnifeAngleToPlant = 150f;
    [SerializeField] private float minMovementAngleToPlant = 140f;
    [SerializeField] private float minSpeedToPlant = 3f;

    [SerializeField] private float delayBeforeNextPlantWhenGrabbed = 0.5f;

    private Vector3 lastPosition;
    private Vector3 actualPosition;

    [SerializeField] private Rigidbody knifeRb;

    [SerializeField] private AudioSource audioSource;

    public bool isPlanted = false;

    void Start()
    {
        lastPosition = transform.position;
        actualPosition = transform.position;
    }


    void Update()
    {
        actualPosition = transform.position;

        float distanceFromLastFrame = Vector3.Distance(actualPosition, lastPosition);
        Vector3 direction = (actualPosition - lastPosition).normalized;


        if (isPlanted == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, maxDistToPlant))
            {
                if (hit.transform.gameObject.CompareTag("Plantable"))
                {
                    float hitDistance = Vector3.Distance(hit.point, transform.position);
                    Debug.Log("Distance de l'émetteur du Raycast au point hit : " + hitDistance);

                    if (hitDistance < maxDistToPlant)
                    {
                        float angleOfKnifeToHitNormal = Vector3.Angle(hit.normal, transform.forward);
                        Debug.Log("Angle entre le couteau et le plan du point hit : " + angleOfKnifeToHitNormal);

                        if (angleOfKnifeToHitNormal > minKnifeAngleToPlant)
                        {
                            float angleOfKnifeMovementToHitNormal = Vector3.Angle(hit.normal, direction);
                            Debug.Log("Angle entre la direction du mouvement du couteau et le plan du point hit : " + angleOfKnifeMovementToHitNormal);

                            if (angleOfKnifeMovementToHitNormal > minMovementAngleToPlant)
                            {
                                float speed = distanceFromLastFrame / Time.deltaTime;                                
                                Debug.Log("Vitesse du couteau : " + speed);

                                if (speed > minSpeedToPlant)
                                {
                                    Vector3 knifePlantVector = transform.forward * 0.2f;
                                    knifeRb.transform.position = hit.point - knifePlantVector; // Pour que le couteau "se plante" un peu dans le mur
                                    knifeRb.constraints = knifeRb.constraints | RigidbodyConstraints.FreezePosition;
                                    knifeRb.constraints = knifeRb.constraints | RigidbodyConstraints.FreezeRotation;
                                    Debug.Log("Le couteau s'est planté. Il s'est enfoncé de " + knifePlantVector + " cm.");
                                    isPlanted = true;
                                    audioSource.Play();
                                }
                            }
                        }
                    }
                }
            }
        }

        lastPosition = actualPosition;
    }


    public void GrabKnife() // Don't forget to call this method when the player grabs the knife!
    {

        knifeRb.constraints &= ~RigidbodyConstraints.FreezePosition;
        knifeRb.constraints &= ~RigidbodyConstraints.FreezeRotation;
        StartCoroutine(DelayBeforeNextPlant());
    }

    private IEnumerator DelayBeforeNextPlant()
    {
        yield return new WaitForSeconds(delayBeforeNextPlantWhenGrabbed);
        isPlanted = false;
    }
}
