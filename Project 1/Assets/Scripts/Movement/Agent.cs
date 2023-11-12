using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    //gets values from physics object
    [SerializeField]
    protected PhysicsObject physicsObject;

    //max speed taken from physics

    //max force that can be applied to object each frame
    [SerializeField]
    float maxForce = 10;

    protected Vector2 pointToRadius;

    protected Vector3 totalForce;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //reset total force
        totalForce = Vector3.zero;

        //any forces applied will be done in each of agent's childs
        CalcSteeringForces();

        //force can not ecceed max force
        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);

        //apply the force to the physics object
        physicsObject.ApplyForce(totalForce);
    }

    protected abstract void CalcSteeringForces();

    protected Vector3 Seek(Vector3 targetPos)
    {
        // Calculate desired velocity: the velocity reqired to take the object to its destination in one frame
        Vector3 desiredVelocity = targetPos - transform.position;

        // scale desired velocity by max speed
        desiredVelocity = desiredVelocity.normalized * physicsObject.MaxSpeed;

        // Calculate seek steering force by subtracting desired velocity by the object's current velocity
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        return seekingForce;
    }

    protected Vector3 Seek(GameObject gameObject)
    {
        //call the other version of seek
        return Seek(gameObject.transform.position);
    }

    protected Vector3 Flee(Vector3 targetPos)
    {
        // Calculate desired velocity: the velocity reqired to take the object to its destination in one frame
        Vector3 desiredVelocity = transform.position - targetPos;

        // scale desired velocity by max speed
        desiredVelocity = desiredVelocity.normalized * physicsObject.MaxSpeed;

        // Calculate seek steering force by subtracting desired velocity by the object's current velocity
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        return seekingForce;
    }

    protected Vector3 Flee(GameObject gameObject)
    {
        //call the other version of seek
        return Flee(gameObject.transform.position);
    }

    //the point the object will be in a certain amount of time
    public Vector3 CalcFuturePosition(float time)
    {
        return physicsObject.Velocity * time + transform.position;
    }

    protected Vector3 Wander(float time, float radius)
    {
        //gets the center of the circle
        Vector3 targetPos = CalcFuturePosition(time);

        //gets a random angle
        float randAngle = Random.Range(0, 2 * Mathf.PI);

        //gets the x and y of a point on the edge of the circle
        targetPos.x = targetPos.x + Mathf.Cos(randAngle) * radius;
        targetPos.y = targetPos.y + Mathf.Sin(randAngle) * radius;

        pointToRadius = targetPos;

        //seek this point
        return Seek(targetPos);
    }
}
