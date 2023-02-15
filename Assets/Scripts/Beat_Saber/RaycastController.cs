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
    [Tooltip("Multiply the length of the raycast for better detection of the cubes. The right value can vary in regard of the length of the saber, the size of the objects du detect, etc.")]
    [SerializeField] private float lengthMultiplier = 4f;
    [Tooltip("Draw lines for showing the raycasts which detect the cubes.")]
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

        float raycastLength = 0.05f + distance * lengthMultiplier;        

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