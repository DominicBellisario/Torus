using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Agent
{
    private GameObject target;

    //spawn manager gives seeker its target when it spawns
    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    protected override void CalcSteeringForces()
    {
        //the target is still in the scene
        if (target != null)
        {
            //applies the steering force to the physics object
            totalForce += Seek(target);
        }
        //moves randomly when player dies
        else
        {
            totalForce += Wander(5, 10);
        }
    }
        

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        //draws a line in the direction of its velocity
        Gizmos.DrawLine(transform.position, physicsObject.Velocity + transform.position);
    }
}
