using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Punches3 : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [Space(5)]
    [SerializeField] private TextMeshProUGUI highestCounterText;
    [SerializeField] private TextMeshProUGUI lowestCounterText;
    [SerializeField] private TextMeshProUGUI movementAmplitudeText;
    [Space(5)]
    [Tooltip("Must be a child object of the camera, positioned at least 1m behind it, to avoid player hands going behind this point.")]
    [SerializeField] private Transform handReferencePoint;

    private float minDiffHandDistance = 0.01f;
    private float lastHandDist;


    private float lastHightestDist = 0;
    private float lastShortestDist = Mathf.Infinity;

    private int highestDistanceCount = 0;
    private int shortestDistanceCount = 0;

    private bool handDistIncreasing;

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
            if (handDist > lastHandDist && !handDistIncreasing)
            {
                lastShortestDist = handDist;
                handDistIncreasing = true;
                shortestDistanceCount++;
                lowestCounterText.text = "Nb of shortest distance: " + shortestDistanceCount;
            }
            
            if (handDist < lastHandDist && handDistIncreasing)
            {
                lastHightestDist = handDist;
                handDistIncreasing = false;
                highestDistanceCount++;
                highestCounterText.text = "Nb of highest distance: " + highestDistanceCount;
            }

            movementAmplitudeText.text = "Movement Amplitude: " + (lastHightestDist - lastShortestDist).ToString("F2");   // N'affichera que deux nombres après la virgule.
        }

            lastHandDist = handDist;
    }
}
