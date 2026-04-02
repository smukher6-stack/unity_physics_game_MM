using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class hingeObject : MonoBehaviour
{

    public float minAngle;
    public float maxAngle;
    public bool useSpring;
    public float springTargetAngle;
    public float springForce;
    public float springDamping;

    public UnityEvent OnReachMax;
    public UnityEvent OnReachMin;
    public float eventThreshold;

    HingeJoint hinge;
    bool maxEventFired = false;
    bool minEventFired = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        ConfigureHinge();

    }

    // Update is called once per frame
    void Update()
    {
        float currenAngle = hinge.angle;
        if(!maxEventFired && currenAngle >= maxAngle -eventThreshold)
        {
            maxEventFired = true;
            minEventFired = false;
            OnReachMax?.Invoke();
            Debug.Log(gameObject.name + "max angle reached");
        }
        else if(!minEventFired && currenAngle <= minAngle + eventThreshold)
        {
            minEventFired = true;
            maxEventFired = false;
            OnReachMin?.Invoke();
            Debug.Log(gameObject.name + "min angle reached");
        }
    }

    void ConfigureHinge()
    {
        JointLimits limits = hinge.limits;
        limits.min = minAngle;
        limits.max = maxAngle;
        limits.bounciness = 0f;
        limits.bounceMinVelocity = 0.2f;
        hinge.limits = limits;
        hinge.useLimits = true;

        if(useSpring == true)
        {
            JointSpring spring = hinge.spring;
            spring.targetPosition = springTargetAngle;
            spring.spring = springForce;
            spring.damper = springDamping;
            hinge.spring = spring;
            hinge.useSpring = true;
        }
        else
        {
            hinge.useSpring = false;
        }
    }

    public void DriveToMax()
    {
        SetMotorTarget(maxAngle);
    }

    public void DriveToMim()
    {
        SetMotorTarget(minAngle);
    }

    void SetMotorTarget(float targetAngle)
    {
        JointMotor motor = hinge.motor;
        motor.targetVelocity = targetAngle > hinge.angle ? 50f : -50f;
        motor.force = 100f;
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;

    }
}
