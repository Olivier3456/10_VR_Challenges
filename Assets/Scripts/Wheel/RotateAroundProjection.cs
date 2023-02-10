using UnityEngine;
using UnityEngine.InputSystem;

public class RotateAroundProjection : MonoBehaviour
{
    [SerializeField] private InputActionReference _gripWithRightHand;       // Grip action r�gl�e en Press > Press Only.
    [SerializeField] private InputActionReference _unGripWithRightHand;     // Grip action sur la m�me g�chette, r�gl�e en Press > Release Only.
    private bool _isGrippedByRightHand;

    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject leftHand;
    

    private int rightHandInColliders = 0;       // La roue a deux colliders, la main ne pourra la tourner que lorsqu'elle sera dans les deux colliders � la fois.

   
    [SerializeField] private GameObject parentObject;
    [SerializeField] private Transform pivot;

  
    private Vector3 pointToProject;
    private Vector3 wheelNormal;
    private Vector3 planePoint;
    private Vector3 handProjection;

    
    public int ClicksCount { get; private set; }

    private Vector3 rightHandLastPosition;

    private void Start()
    {    
        EnableInputActions();
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
        pointToProject = rightHand.transform.position;
        planePoint = transform.position;
        handProjection = pointToProject - (Vector3.Dot(wheelNormal, pointToProject - planePoint) / Vector3.Dot(wheelNormal, wheelNormal)) * wheelNormal;
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

    private void OnTriggerEnter(Collider other)     // M�thode appel�e chaque fois que quelque chose entre dans un des deux colliders.
    {
        // Pour que la main puisse agir sur la roue, on veut que les deux triggers soient d�clench�s :
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
        // La main droite peut faire bouger la roue si elle est dans les deux colliders, et si le bouton du grip est press�.
        if (rightHandInColliders == 2 && _isGrippedByRightHand)
        {
            ProjectHandPositionToWheelPlan();
            Vector3 directionFromPivotToLastHandProjection = Vector3.Normalize(pivot.position - handProjection);
            Vector3 directionFromPivotToNewHandProjection = Vector3.Normalize(pivot.position - rightHandLastPosition);

            float angle = Vector3.SignedAngle(directionFromPivotToNewHandProjection, directionFromPivotToLastHandProjection, Vector3.forward);

            // La roue va tourner autour de son propre axe, sp�cifi� comme �tant local (transform.TransformDirection) :            
            transform.RotateAround(transform.position, transform.TransformDirection(Vector3.up), angle);

            rightHandLastPosition = handProjection;            
        }
    }    
}
