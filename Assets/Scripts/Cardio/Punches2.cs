using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Punches2 : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [Space(5)]
    [SerializeField] private TextMeshProUGUI highestCounterText;
    [SerializeField] private TextMeshProUGUI lowestCounterText;
    [SerializeField] private TextMeshProUGUI movementAmplitudeText;
    [Space(5)]
    [Tooltip("Must be a child object of the camera, positioned about 1m behind it.")]
    [SerializeField] private Transform handReferencePoint;

    float highestDistance = 0;
    float shortestDistance = 1000;
    float handDist;
    float lastHandDist;

    bool highestReached;
    bool shortestReached;

    int highestDistanceCount;
    int shortestDistanceCount;
    

    void Start()
    {
        lastHandDist = Vector3.Distance(hand.transform.position, handReferencePoint.position);
    }


    void Update()
    {
        handDist = Vector3.Distance(hand.transform.position, handReferencePoint.position);

        if (handDist > highestDistance)
        {
            highestDistance = handDist;
        }

        if (handDist < shortestDistance)
        {
            shortestDistance = handDist;
        }

        if (handDist > highestDistance * 0.9f) // Entr�e dans la zone highest distance.
        {
            shortestReached = false;    // Apr�s �tre pass�e dans la zone de distance max, la main pourra r�enregistrer une distance min.

            if (handDist < lastHandDist && !highestReached)     // Si la main se rapproche, c'est qu'elle a atteint sa distance max. Ce max ne pourra �tre comptabilis� qu'une seule fois tant que la main ne sortira pas de la zone highest distance.
            {
                highestDistance = handDist;
                highestReached = true;
                highestDistanceCount++;
                highestCounterText.text = "Nb of highest distance: " + highestDistanceCount;
            }
        }
        else  if (handDist < shortestDistance * 0.9f)
        {
            highestReached = false;   // Apr�s �tre pass�e dans la zone de distance min, la main pourra r�enregistrer une distance max.

            if (handDist > lastHandDist && !shortestReached)     // Si la main s'�loigne, c'est qu'elle a atteint sa distance min. Ce min ne pourra �tre comptabilis� qu'une seule fois tant que la main ne sortira pas de la zone shortest distance.
            {
                shortestDistance = handDist;
                shortestReached = true;
                shortestDistanceCount++;
                lowestCounterText.text = "Nb of shortest distance: " + shortestDistanceCount;
            }
        }



        lastHandDist = handDist;
    }
}
