using UnityEngine;
using UnityEngine.InputSystem;

public class RotateAroundProjection : MonoBehaviour
{
    [Tooltip("Interactions mode of the input action must be set in: Press > Press Only.")]
    [SerializeField] private InputActionReference _gripWithRightHand;
    [Tooltip("Interactions mode of the input action must be set in mode: Press > Release Only.")]
    [SerializeField] private InputActionReference _unGripWithRightHand;
    private bool _isGrippedByRightHand;

    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;

    private int rightHandInColliders = 0;       // La roue a deux colliders, la main ne pourra la tourner que lorsqu'elle sera dans les deux colliders à la fois.

    [Tooltip("The wheel must be in a parent object that will be moved in the scene.")]
    [SerializeField] private GameObject parentObject;

    private Vector3 handPosition;
    private Vector3 wheelNormal;
    private Vector3 planePoint;
    private Vector3 handProjection;

    private Vector3 rightHandLastPosition;

    [Tooltip("Must be on the plan of the wheel, at its start rotation, and be a child of the wheel.")]
    [SerializeField] private Transform needle;
    private WheelNeedle wheelNeedle;
    private Vector3 startOrientation;
    private Vector3 actualOrientation;
    public float actualAngle { get; private set; }


    private void Start()
    {
        EnableInputActions();
        wheelNeedle = needle.GetComponent<WheelNeedle>();
        startOrientation = Vector3.Normalize(needle.position - transform.position);
        actualOrientation = startOrientation;
        actualAngle = 0;
    }

    private void EnableInputActions()
    {
        _gripWithRightHand.action.Enable();
        _gripWithRightHand.action.performed += GripTheWheel;
        _unGripWithRightHand.action.Enable();
        _unGripWithRightHand.action.performed += UnGripTheWheel;
    }

    private void ProjectHandPositionToWheelPlan()
    {
        wheelNormal = parentObject.transform.forward;
        handPosition = rightHand.transform.position;
        planePoint = transform.position;
        handProjection = handPosition - (Vector3.Dot(wheelNormal, handPosition - planePoint) / Vector3.Dot(wheelNormal, wheelNormal)) * wheelNormal;
    }

    private void GripTheWheel(InputAction.CallbackContext obj)
    {
        _isGrippedByRightHand = true;
        ProjectHandPositionToWheelPlan();
        rightHandLastPosition = handProjection;
    }
    private void UnGripTheWheel(InputAction.CallbackContext obj)
    {
        _isGrippedByRightHand = false;
    }

    private void OnTriggerEnter(Collider other)     // Méthode appelée chaque fois que quelque chose entre dans un des deux colliders.
    {
        // Pour que la main puisse agir sur la roue, on veut que les deux triggers soient déclenchés :
        if (other.name == rightHand.name)
        {
            rightHandInColliders++;
            if (rightHandInColliders == 2)
            {
                ProjectHandPositionToWheelPlan();
                rightHandLastPosition = handProjection;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == rightHand.name) rightHandInColliders--;
    }


    private void OnTriggerStay(Collider other)
    {
        // La main droite peut faire bouger la roue si elle est dans les deux colliders, et si le bouton du grip est pressé.
        if (rightHandInColliders == 2 && _isGrippedByRightHand)
        {
            ProjectHandPositionToWheelPlan();
            Vector3 directionFromPivotToLastHandProjection = Vector3.Normalize(transform.position - handProjection);
            Vector3 directionFromPivotToNewHandProjection = Vector3.Normalize(transform.position - rightHandLastPosition);

            float angle = Vector3.SignedAngle(directionFromPivotToNewHandProjection, directionFromPivotToLastHandProjection, Vector3.forward);            

            // La roue va tourner autour de son propre axe, spécifié comme étant local (transform.TransformDirection) :            
            transform.RotateAround(transform.position, transform.TransformDirection(Vector3.up), angle);

            rightHandLastPosition = handProjection;

            actualOrientation = Vector3.Normalize(needle.position - transform.position);
            actualAngle = Vector3.SignedAngle(actualOrientation, startOrientation, Vector3.forward);
            wheelNeedle.VerifyGraduation(angle, actualAngle);
        }
    }
}
