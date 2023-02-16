using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RaycastController : MonoBehaviour
{
    [Tooltip("Saber_Red or Saber_blue (see Cube class, method TouchedByRaycast())")]
    [SerializeField] private string saberName;

    private Vector3 lastPosition;
    private Vector3 actualPosition;

    [Tooltip("The saber itself.")]
    [SerializeField] VibrationsController vibrationsController;
    [Tooltip("Multiply the length of the raycast for better detection of the cubes. The right value may vary in regard of the length of the saber, the size of the objects to detect, etc.")]
    [SerializeField] private float lengthMultiplier = 4f;
    [Tooltip("The minimal length of the raycast when the saber moves slowly.")]
    [SerializeField] private float minimalLength = 0.05f;
    [Tooltip("Draw the raycast in scene view.")]
    public bool debugMod = true;

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

        float raycastLength = distance * lengthMultiplier;
        if (raycastLength < minimalLength) raycastLength = minimalLength;
       
        if (debugMod) Debug.DrawLine(actualPosition, actualPosition + direction * raycastLength, UnityEngine.Color.green);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastLength))
        {
            Cube cube = hit.transform.gameObject.GetComponent<Cube>();

            if (cube != null)
            {
                cube.TouchedByRaycast(saberName, direction);
                vibrationsController.SendHapticImpulse();
            }
        }
        lastPosition = actualPosition;
    }
}