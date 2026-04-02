using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;

public class pressurePlate : MonoBehaviour
{

    //weight settings
    //total weight for plate activation
    public float weightThreshold = 5f;

    //if true, plate stays on after object removed
    public bool lockOnActivate = false;

    public UnityEvent onActivated;
    public UnityEvent onDeactivated;

    public Transform plate;
    public float pressDepth = 0.05f;

    float currentWeight = 0f;
    bool isActivated = false;
    bool isLocked = false;

    Vector3 plateResetPos;
    Vector3 platePressedPos;

    //hash set later
    //lmao hash set now

    HashSet<physicsObject> objectsOnPlate = new HashSet<physicsObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(plate != null)
        {
            plateResetPos = plate.localPosition;
            platePressedPos = plateResetPos + Vector3.down * pressDepth;
        }
    }

    //a hot new collider has entered the villa
    //check for physics obj
    private void OnTriggerEnter(Collider other)
    {
        physicsObject physObj = other.GetComponent<physicsObject>();
        if (physObj == null) return;

        if (physObj.isHeld) return;

        //simple version
        //currentWeight += physObj.puzzleWeight;
        //Debug.Log($"{other.gameObject.name} has entered the villa. total weight {currentWeight}");
        //CheckActivation();

        if (objectsOnPlate.Add(physObj))
        {
            currentWeight += physObj.puzzleWeight;
            CheckActivation();  
        }

    }

    private void OnTriggerStay(Collider other)
    {
        physicsObject physicsObj = other.GetComponent<physicsObject>();
        if(physicsObj == null) return;

        if(physicsObj.isHeld) return;

        if (objectsOnPlate.Add(physicsObj))
        {
            currentWeight += physicsObj.puzzleWeight;
            CheckActivation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(isLocked) return;
        physicsObject physicsObj = other.GetComponent<physicsObject>();
        if(physicsObj == null) return;

        if (objectsOnPlate.Remove(physicsObj))
        {
            currentWeight -= physicsObj.puzzleWeight;
            currentWeight = Mathf.Max(0f, currentWeight);
            CheckDeactivation();

        }
    }
    // Update is called once per frame
    void CheckActivation()
    {
        if(!isActivated && currentWeight >= weightThreshold)
        {
            isActivated = true;
            if(lockOnActivate) isLocked = true;
            onActivated.Invoke();

            Debug.Log("damn didn't know the plate and i were siblings");

            if(plate != null)
            {
                plate.localPosition = plateResetPos;
            }
        }
    }

    void CheckDeactivation()
    {
        if(isActivated && !isLocked && currentWeight < weightThreshold)
        {
            isActivated = false;
            onDeactivated.Invoke();
            Debug.Log("glad the plate got therapy");

            if(plate != null)
            {
                plate.localPosition = plateResetPos;
            }
        }
    }
}
