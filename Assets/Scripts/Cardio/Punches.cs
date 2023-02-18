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
    [SerializeField] private TextMeshProUGUI movementAmplitudeText;
    [Space(5)]
    [Tooltip("Must be a child object of the camera, positioned about 1m behind it.")]
    [SerializeField] private Transform handReferencePoint;
   
    private float timeTrack = 0.1f;
    private float minDiffHandDistance = 0.01f;
    
    private Queue<float> lastDistances = new Queue<float>();

    private int frame = 0;
       
    private float lastHandDist;

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
        lastHandDist = Vector3.Distance(hand.transform.position, handReferencePoint.position);
    }

    
    void Update()
    {      
        float handDist = Vector3.Distance(hand.transform.position, handReferencePoint.position);

        float diffHandDistance = Mathf.Abs(handDist - lastHandDist);
        Debug.Log("diffHandDistance = " + diffHandDistance);

        if (diffHandDistance > minDiffHandDistance)          // Eliminate false highest/shortest counts when the hand moves too slow.
        {
            CollectLastPosition(handDist);

            float maxDist = 0;
            float minDist = 10000;
            FindMinAndMaxDistancesInLastPositions(ref maxDist, ref minDist);

            LooksIfTheHandMovesAwayOrTowards(handDist, maxDist, minDist);

            CounterOfExtremeDistances();

            movementAmplitudeText.text = "Movement Amplitude: " + (lastHightestDist - lastShortestDist).ToString("F2");   // N'affichera que deux nombres après la virgule.
        }
        
        lastHandDist = handDist;
    }



    private void CounterOfExtremeDistances()
    {
        float instantTimeTrack = timeTrack / Time.deltaTime;

        if (countFromLastHighestDist >= instantTimeTrack && !highestCountAlreadyDone)
        {
            highestCountAlreadyDone = true;
            highestDistanceCount++;
            highestCounterText.text = "Nb of highest distance: " + highestDistanceCount;
        }
        if (countFromLastShortestDist >= instantTimeTrack && !shortestCountAlreadyDone)
        {
            shortestCountAlreadyDone = true;
            shortestDistanceCount++;
            lowestCounterText.text = "Nb of shortest distance: " + shortestDistanceCount;
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
        if (frame > 4)     // We will always have the same number of frames in the queue.
        {
            lastDistances.Dequeue();

            Debug.Log("Images dans la queue : " + lastDistances.Count);
        }
    }
}
