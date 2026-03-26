using UnityEngine;
using UnityEngine.InputSystem;

public class objectGrabber : MonoBehaviour
{

    [Header("Grab Settings")]
    [Tooltip("How far way the player can grab from")]
    public float grabRange = 4;

    [Tooltip("How fast held object moves to hold point. higher = snappier")]
    public float holdSmoothing = 15;

    //point in front of camera where object is held
    public Transform holdPoint;

    //how much force is applied when throwing
    public float throwForce = 15;

    private Rigidbody heldObject;
    private bool isHolding = false;
    private interactableObject currentHighlight;
    void FixedUpdate()
    {
        if (isHolding && heldObject != null) MoveHeldObject();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHighlight();

    }

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position,transform.forward * grabRange, Color.aliceBlue, 0.5f);

        if(Physics.Raycast(ray, out hit, grabRange))
        {
            interactableObject interactable = hit.collider.GetComponent<interactableObject>();
            if(interactable != null)
            {
                heldObject = hit.collider.GetComponent<Rigidbody>();
                if(heldObject != null)
                {
                    heldObject.useGravity = false;

                    heldObject.freezeRotation = true; 
                    heldObject.linearVelocity = Vector3.zero;
                    heldObject.angularVelocity = Vector3.zero;

                    interactable.Unhighlight();
                    currentHighlight = null;

                    isHolding = true;
                    Debug.Log("yoink");
                }
            }
        }

    }

    void MoveHeldObject()
    {
        Vector3 targetpos = holdPoint.position;
        Vector3 currentpos = heldObject.position;

        Vector3 newPos = Vector3.Lerp(currentpos, targetpos, holdSmoothing * Time.fixedDeltaTime);

        heldObject.MovePosition(newPos);

    }

    void DropObject()
    {
        if (heldObject == null) return;

        heldObject.useGravity = true;
        heldObject.freezeRotation = false;
        heldObject = null;
        isHolding = false;

        Debug.Log("oops i dropped it");
    }

    void ThrowObject()
    {
        if (heldObject == null) return;

        heldObject.useGravity = true;
        heldObject.freezeRotation = false;
        heldObject.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        heldObject = null;
        isHolding = false;

        Debug.Log("YEET");


    }

    public void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (isHolding) DropObject();
        else TryGrab();
        {

        }
    }

    public void OnThrowPerformed(InputAction.CallbackContext context)
    {
        if (isHolding) ThrowObject();
    }

    void UpdateHighlight()
    {
        if(isHolding) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * grabRange, Color.peachPuff);

        if(Physics.Raycast(ray, out hit, grabRange))
        {
            interactableObject interactable = hit.collider.GetComponent<interactableObject>();
            if (interactable != null)
            {
                if(currentHighlight != null && currentHighlight != interactable)
                {
                    currentHighlight.Unhighlight();
                    Debug.Log("whoops got your highlight");
                }
                interactable.Highlight();
                currentHighlight = interactable;
                return;
            }
        }

        if(currentHighlight != null)
        {
            currentHighlight.Unhighlight();
            currentHighlight = null;
        }
    }
}
