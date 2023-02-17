using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Punches : MonoBehaviour
{
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform leftHand;
    [Space(5)]
    [SerializeField] private TextMeshProUGUI highestCounterText;
    [SerializeField] private TextMeshProUGUI lowestCounterText;
    [Space(5)]
    [SerializeField] private Transform camera;
    [Space(10)]
    [SerializeField] private float timeTrack = 0.1f;

    private Queue<float> rightHandLastDistances = new Queue<float>();
    private Queue<float> leftHandLastDistances = new Queue<float>();

    private int frame = 0;

    private Vector3 lastRightHandPosition;
        
    private float lastHightestDistRight;
    private float lastShortestDistRight;

    private int countFromLastHighestDistRight = 0;
    private int countFromLastShortestDistRight = 0;

    private int highestDistanceRightCount = 0;
    private int shortestDistanceRightCount = 0;

    private bool highestCountAlready;
    private bool shortestCountAlready;

    


    void Start()
    {
        timeTrack /= Time.deltaTime;    
    }


    void Update()
    {
        float handDistanceFromLastFrame = Vector3.Distance(rightHand.position, lastRightHandPosition);
        if (handDistanceFromLastFrame > 0.01f)      // Elimine les faux comptages de highest/shortest distances quand la main ne bouge pas ou presque.
        {

            float rightHandDistance = Vector3.Distance(rightHand.transform.position, camera.position);
            float leftHandDistance = Vector3.Distance(leftHand.transform.position, camera.position);

            rightHandLastDistances.Enqueue(rightHandDistance);
            leftHandLastDistances.Enqueue(leftHandDistance);

            frame++;
            if (frame >= timeTrack)     // Nous aurons donc toujours le même nombre d'images dans les queues, l'entier supérieur le plus proche de timeTrack (je crois).
            {
                rightHandLastDistances.Dequeue();
                leftHandLastDistances.Dequeue();
            }

            float maxDistRightHand = 0;
            float minDistRightHand = 10000;
            foreach (float distance in rightHandLastDistances)
            {
                if (distance <= minDistRightHand) minDistRightHand = distance;
                else if (distance >= maxDistRightHand) maxDistRightHand = distance;
            }


            if (rightHandDistance == maxDistRightHand)
            {
                lastHightestDistRight = rightHandDistance;
                countFromLastHighestDistRight = 0;
                countFromLastShortestDistRight++;
                highestCountAlready = false;
            }
            else if (rightHandDistance == minDistRightHand)
            {
                lastShortestDistRight = rightHandDistance;
                countFromLastHighestDistRight++;
                countFromLastShortestDistRight = 0;
                shortestCountAlready = false;
            }
            else
            {
                countFromLastHighestDistRight++;
                countFromLastShortestDistRight++;
            }


            if (countFromLastHighestDistRight >= timeTrack && !highestCountAlready)
            {
                highestCountAlready = true;
                highestDistanceRightCount++;
                highestCounterText.text = "Nb highest dist: " + highestDistanceRightCount;
            }
            if (countFromLastShortestDistRight >= timeTrack && !shortestCountAlready)
            {
                shortestCountAlready = true;
                shortestDistanceRightCount++;
                lowestCounterText.text = "Nb highest dist: " + shortestDistanceRightCount;
            }



        }
        lastRightHandPosition = rightHand.position;
    }
}
