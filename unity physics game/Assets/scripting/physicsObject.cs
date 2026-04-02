using UnityEngine;

public class physicsObject : MonoBehaviour
{
    [Header("Mass and Motion: aka A Total Drag")]
    //weight in kilos affects force needed to move 
    [Range(0.1f, 100f)]
    public float mass = 1f;

    //liner drag: how quickly it slows dowin in the air
    [Range(0f, 10f)]
    public float dragPath = 0.5f;

    //angular drag: how quickly spinning slows down
    [Range(0f, 10f)]
    public float dragContour = 0.5f;

    [Header("Surface Properties")]
    //bounciness
    [Range(0f, 1f)]
    public float bouncyCastle = 0f;

    [Range(0f, 1f)]
    public float friction = 0.6f;

    [Header("Puzzle Time Yippee")]
    //pressure plate stuff
    //defaults to mass
    public float puzzleWeight = -1f;
    
    Rigidbody rb;
    PhysicsMaterial physics;
    public bool isHeld;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //function time
        ApplyRigidBodySettings();
        ApplySurfaceSettings();                                                                    
    }

    void ApplyRigidBodySettings()
    {
        rb.mass = mass;
        rb.linearDamping = dragPath;
        rb.angularDamping = dragContour;
    }

    //physics material in unity control bounce & friction
    //assign physics material created at runtime
    void ApplySurfaceSettings()
    {
        physics = new PhysicsMaterial(gameObject.name);
        physics.bounciness = bouncyCastle;
        physics.dynamicFriction = friction;
        physics.staticFriction = friction;
        //YIPPEE THERES MORE
        //combineMode.maximum = higher friction of the two
        //colliding object wins. good default for solid objects. yippee

        physics.frictionCombine = PhysicsMaterialCombine.Average;
        physics.bounceCombine = PhysicsMaterialCombine.Maximum;

        //assigned material at awake

        Collider col = GetComponent<Collider>();
        if(col != null )
        {
            col.material = physics;
        }
    }
    //preview in editor
    //value changes in play mode stay without restarting

    private void OnValidate()
    {
        //runs in editor whenever an inspector value changes
        if(rb != null) ApplyRigidBodySettings();
    }
}
