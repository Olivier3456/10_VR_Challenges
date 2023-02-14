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

    [SerializeField] GameObject testRaycast;



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

        if (testRaycast != null) testRaycast.transform.position = transform.position + (direction * distance * lengthMultiplier);

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, direction, out hit, distance * lengthMultiplier))
        //{
        //    CubeDirectionalCollider cubeDirectionalCollider = hit.transform.gameObject.GetComponent<CubeDirectionalCollider>();

        //    if (cubeDirectionalCollider != null)
        //    {
        //        cubeDirectionalCollider.TouchedByRaycast(saberName);
        //    }
        //}


        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, distance * lengthMultiplier))
        {
            Cube cube = hit.transform.gameObject.GetComponent<Cube>();

            if (cube != null)
            {
                cube.TouchedByRaycast(saberName, transform);
            }
        }

        lastPosition = actualPosition;
    }
}