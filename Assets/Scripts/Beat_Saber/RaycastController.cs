using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private float lengthMultiplier = 1f; // Facteur de multiplication de la longueur du raycast.

    [SerializeField] private string saberName;

    private Vector3 lastPosition;
    private Vector3 actualPosition;

    //  [SerializeField] GameObject testRaycast;



    void Start()
    {
        lastPosition = transform.position;
        actualPosition = transform.position;
    }

    void Update()
    {
        actualPosition = transform.position;

        Vector3 direction = (actualPosition - lastPosition).normalized;
        float distance = Vector3.Distance(actualPosition, lastPosition);

        //   testRaycast.transform.position = transform.position + (direction * distance * lengthMultiplier);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, distance * lengthMultiplier))
        {
            Cube cube = hit.transform.gameObject.GetComponent<Cube>();
            if (cube != null)
            {
                cube.TouchedFromRightDirection(saberName);
                Debug.Log("RaycastController de " + saberName + " appelle cube.TouchedFromRightDirection()");
            }
            else
            {
                CubeDirectionalCollider cubeDirectionalCollider = hit.transform.gameObject.GetComponent<CubeDirectionalCollider>();

                if (cubeDirectionalCollider != null)
                {
                    cubeDirectionalCollider.TouchedByRaycast(saberName);
                    Debug.Log("RaycastController de " + saberName + " appelle cubeDirectionalCollider.TouchedFromRightDirection()");
                }
            }

        }

        lastPosition = actualPosition;
    }
}