using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Punches : MonoBehaviour
{
    [SerializeField] private Transform hand;    
    [Space(5)]
    [SerializeField] private TextMeshProUGUI highestCounterText;
    [SerializeField] private TextMeshProUGUI lowestCounterText;
    [Space(5)]
    [SerializeField] private Transform camera;
    [Space(10)]
    [SerializeField] private float timeTrack = 0.1f;
    [SerializeField] private float minDistBetweenTwoMesures = 0.01f;


    private Queue<float> lastDistances = new Queue<float>();  

    private int frame = 0;

    private Vector3 lastPosition;
        
    private float lastHightestDist;
    private float lastShortestDist;

    private int countFromLastHighestDist = 0;
    private int countFromLastShortestDist = 0;

    private int highestDistanceCount = 0;
    private int shortestDistanceCount = 0;

    private bool highestCountAlreadyDone;
    private bool shortestCountAlreadyDone;

    


    void Start()
    {
        timeTrack /= Time.deltaTime;    
    }


    void Update()
    {
        float handDistanceFromLastFrame = Vector3.Distance(hand.position, lastPosition);
        if (handDistanceFromLastFrame > minDistBetweenTwoMesures)      // Eliminate false highest/shortest counts when the hand moves too slow.
        {

            float handDist = Vector3.Distance(hand.transform.position, camera.position);

            CollectLastPosition(handDist);

            float maxDist = 0;
            float minDist = 10000;
            FindMinAndMaxDistancesInLastPositions(ref maxDist, ref minDist);

            LooksIfTheHandMovesAwayOrTowards(handDist, maxDist, minDist);

            CounterOfMinOrMaxDistances();
        }
        lastPosition = hand.position;
    }

   

    private void CounterOfMinOrMaxDistances()
    {
        if (countFromLastHighestDist >= timeTrack && !highestCountAlreadyDone)
        {
            highestCountAlreadyDone = true;
            highestDistanceCount++;
            highestCounterText.text = "Nb highest dist: " + highestDistanceCount;
        }
        if (countFromLastShortestDist >= timeTrack && !shortestCountAlreadyDone)
        {
            shortestCountAlreadyDone = true;
            shortestDistanceCount++;
            lowestCounterText.text = "Nb highest dist: " + shortestDistanceCount;
        }
    }

    private void LooksIfTheHandMovesAwayOrTowards(float handDist, float maxDist, float minDist)
    {
        if (handDist == maxDist)
        {
            lastHightestDist = handDist;
            countFromLastHighestDist = 0;
            countFromLastShortestDist++;
            highestCountAlreadyDone = false;
        }
        else if (handDist == minDist)
        {
            lastShortestDist = handDist;
            countFromLastHighestDist++;
            countFromLastShortestDist = 0;
            shortestCountAlreadyDone = false;
        }
        else
        {
            countFromLastHighestDist++;
            countFromLastShortestDist++;
        }
    }


    private void FindMinAndMaxDistancesInLastPositions(ref float maxDist, ref float minDist)
    {
        foreach (float distance in lastDistances)
        {
            if (distance <= minDist) minDist = distance;
            else if (distance >= maxDist) maxDist = distance;
        }
    }


    private void CollectLastPosition(float handDistance)
    {
        lastDistances.Enqueue(handDistance);

        frame++;
        if (frame >= timeTrack)     // Nous aurons donc toujours le même nombre d'images dans les queues, l'entier supérieur le plus proche de timeTrack (je crois).
        {
            lastDistances.Dequeue();
        }
    }
}
