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
    [SerializeField] private TextMeshProUGUI handSpeedText;
    [Space(5)]
    [Tooltip("Must be a child object of the camera, positioned at least 1m behind it, to avoid player hands going behind this point.")]
    [SerializeField] private Transform handReferencePoint;

    private float minDiffHandDistance = 0.01f;
    private float lastHandDist;

    private float speed = 0;
    [SerializeField] float speedDisplayFrequency = 0.1f;

    private float lastHightestDist = 0;
    private float lastShortestDist = Mathf.Infinity;

    private int highestDistanceCount = 0;
    private int shortestDistanceCount = 0;

    private bool handDistIncreasing;

    void Start()
    {
        lastHandDist = Vector3.Distance(hand.transform.position, handReferencePoint.position);        
        StartCoroutine(DisplaySpeed());     // Limits the frequency of speed display.
    }


    void Update()
    {
        float handDist = Vector3.Distance(hand.transform.position, handReferencePoint.position);

        float diffHandDistance = Mathf.Abs(handDist - lastHandDist);
        
        if (diffHandDistance > minDiffHandDistance)          // Eliminate false highest/shortest counts when the hand moves too slow.
        {
            speed = (diffHandDistance / Time.deltaTime) * 3.6f;

            if (!handDistIncreasing && handDist > lastHandDist)
            {
                lastShortestDist = handDist;
                handDistIncreasing = true;
                shortestDistanceCount++;
                lowestCounterText.text = "Returns: " + shortestDistanceCount;
                DisplayAmplitude();
            }

            if (handDistIncreasing && handDist < lastHandDist)
            {
                lastHightestDist = handDist;
                handDistIncreasing = false;
                highestDistanceCount++;
                highestCounterText.text = "Punches: " + highestDistanceCount;
                DisplayAmplitude();
            }
        }

        lastHandDist = handDist;
    }

    private void DisplayAmplitude()
    {
        movementAmplitudeText.text = "Amplitude: " + ((lastHightestDist - lastShortestDist) * 100).ToString("F0") + " cm";
    }

    IEnumerator DisplaySpeed()
    {
        yield return new WaitForSeconds(speedDisplayFrequency);
        handSpeedText.text = "Hand speed: " + speed.ToString("F0") + " km/h";
        StartCoroutine(DisplaySpeed());
    }


}
