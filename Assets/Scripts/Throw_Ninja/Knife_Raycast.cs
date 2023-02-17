using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_Raycast : MonoBehaviour
{
    [SerializeField] private float maxDistToPlant = 0.05f;
    [SerializeField] private float minKnifeAngleToPlant = 150f;
    [SerializeField] private float minMovementAngleToPlant = 140f;
    [SerializeField] private float minSpeedToPlant = 3f;
    [SerializeField] private float maxAngleBetweenKnifeAndItsMovement = 20f;

    [SerializeField] private float delayBeforeNextPlantWhenGrabbed = 0.5f;

    private Vector3 lastPosition;
    private Vector3 actualPosition;

    private Vector3 lastLastPosition;

    [SerializeField] private Rigidbody knifeRb;

    [SerializeField] private AudioSource audioSource;

    public bool isPlanted = false;

    public void GrabKnife() // Don't forget to call this method when the player grabs the knife!
    {

        knifeRb.constraints &= ~RigidbodyConstraints.FreezePosition;
        knifeRb.constraints &= ~RigidbodyConstraints.FreezeRotation;
        StartCoroutine(DelayBeforeNextPlant());
    }


    void Start()
    {
        lastPosition = transform.position;        
        actualPosition = transform.position;

        lastLastPosition = transform.position;
    }


    void Update()
    {
        actualPosition = transform.position;
        

        float distanceFromLastFrame = Vector3.Distance(actualPosition, lastPosition);
        float lastDistanceFromLastFrame = Vector3.Distance(lastLastPosition, lastPosition);     // Un test pour voir si l'appréciation de la vitesse du couteau est plus fiable si on prend la vitesse une frame plus tôt (si jamais la collision a déjà ralenti le couteau lors du calcul).

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
                                float angleFromKnifeToKnifeMovement = Vector3.Angle(direction, transform.forward);
                                Debug.Log("Angle entre le couteau et son mouvement : " + angleFromKnifeToKnifeMovement);

                                if (angleFromKnifeToKnifeMovement < maxAngleBetweenKnifeAndItsMovement)
                                {
                                    float speed = distanceFromLastFrame / Time.deltaTime;
                                    float lastSpeed = lastDistanceFromLastFrame / Time.deltaTime;


                                    Debug.Log("Vitesse du couteau (avant-dernière image) : " + lastSpeed);
                                    Debug.Log("Vitesse du couteau (dernière image) : " + speed);

                                    if (speed > minSpeedToPlant)
                                    {
                                        float halfKnifeLength = 0.24f;
                                        Vector3 knifePlantVector = transform.forward * ((halfKnifeLength) - (speed * 0.01f));
                                        knifeRb.transform.position = hit.point - knifePlantVector; // Pour que le couteau "se plante" un peu dans le mur
                                        knifeRb.constraints = knifeRb.constraints | RigidbodyConstraints.FreezePosition;
                                        knifeRb.constraints = knifeRb.constraints | RigidbodyConstraints.FreezeRotation;
                                        Debug.Log("LE COUTEAU S'EST PLANTé. IL S'EST ENFONCé DE " + knifePlantVector.magnitude + " CM.");
                                        isPlanted = true;
                                        audioSource.Play();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        lastLastPosition = lastPosition;

        lastPosition = actualPosition;
    }
        

    private IEnumerator DelayBeforeNextPlant()
    {
        yield return new WaitForSeconds(delayBeforeNextPlantWhenGrabbed);
        isPlanted = false;
    }
}
