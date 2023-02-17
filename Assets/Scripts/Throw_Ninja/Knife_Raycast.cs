using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife_Raycast : MonoBehaviour
{
    [SerializeField] private float maxDistToPlant = 0.05f;
    [SerializeField] private float minKnifeAngleToPlant = 150f;
    [SerializeField] private float minMovementAngleToPlant = 140f;
    [SerializeField] private float minSpeedToPlant = 3f;
    [SerializeField] private float maxAngleBetweenKnifeAndItsMovement = 25f;

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

        if (isPlanted == false) // Pour mieux faire, au lieu de cette cascade de if, il faudrait que chaque critère influe sur un même nombre
                                // dont la valeur finale déterminera si le couteau se plante ou non. Par exemple, la vitesse, si elle est
                                // très importante, doit pouvoir permettre au couteau de se planter, même si ses angles ne sont pas optimaux.
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistToPlant))
            {
                if (hit.transform.gameObject.CompareTag("Plantable"))
                {
                    float distanceFromLastFrame = Vector3.Distance(actualPosition, lastPosition);
                    float lastDistanceFromLastFrame = Vector3.Distance(lastLastPosition, lastPosition);     // Un test pour voir si l'appréciation de la vitesse du couteau est plus fiable si on prend la vitesse une frame plus tôt (si jamais la collision a déjà ralenti le couteau lors du calcul).

                    Vector3 MovementDirection = (actualPosition - lastPosition).normalized;                    

                    float speed = distanceFromLastFrame / Time.deltaTime;
                    float lastSpeed = lastDistanceFromLastFrame / Time.deltaTime;
                    
                    float highestSpeedOfTheTree;    // Nous prendrons la plus grande de ces trois vitesses, qui varient beaucoup entre elles au même moment, j'ignore pourquoi.
                    if (speed >= lastSpeed) highestSpeedOfTheTree = speed;
                    else highestSpeedOfTheTree = lastSpeed;
                    if (highestSpeedOfTheTree <= knifeRb.velocity.magnitude) highestSpeedOfTheTree = knifeRb.velocity.magnitude;
                    speed = highestSpeedOfTheTree;
                    Debug.Log("Vitesse du couteau (avant-dernière image) : " + lastSpeed + ". Vitesse du couteau (dernière image) : " + speed + ". Vitesse du rigidbody : " + knifeRb.velocity.magnitude + " Valeur retenue : " + speed);

                    if (speed > minSpeedToPlant)
                    {
                        float angleFromKnifeToHitNormal = Vector3.Angle(hit.normal, transform.forward);
                        Debug.Log("Angle entre le couteau et le plan du point hit : " + angleFromKnifeToHitNormal);

                        if (angleFromKnifeToHitNormal + speed * 2 > minKnifeAngleToPlant)
                        {
                            float angleFromKnifeMovementToHitNormal = Vector3.Angle(hit.normal, MovementDirection);
                            Debug.Log("Angle entre la direction du mouvement du couteau et le plan du point hit : " + angleFromKnifeMovementToHitNormal);

                            if (angleFromKnifeMovementToHitNormal + speed * 2 > minMovementAngleToPlant)
                            {
                                float angleFromKnifeToKnifeMovement = Vector3.Angle(MovementDirection, transform.forward);
                                Debug.Log("Angle entre le couteau et son mouvement : " + angleFromKnifeToKnifeMovement);

                                if (angleFromKnifeToKnifeMovement + speed * 2 < maxAngleBetweenKnifeAndItsMovement)
                                {
                                    float halfKnifeLength = 0.24f;
                                    Vector3 knifePlantVector = transform.forward * (halfKnifeLength - (speed * 0.01f));
                                    knifeRb.transform.position = hit.point - knifePlantVector; // Pour que le couteau "se plante" un peu dans le mur
                                    knifeRb.constraints = knifeRb.constraints | RigidbodyConstraints.FreezePosition;
                                    knifeRb.constraints = knifeRb.constraints | RigidbodyConstraints.FreezeRotation;
                                    Debug.Log("LE COUTEAU S'EST PLANTé. IL S'EST ENFONCé DE " + knifePlantVector.magnitude * 100 + " CM.");
                                    isPlanted = true;
                                    audioSource.Play();
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
