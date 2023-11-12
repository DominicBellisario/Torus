using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PhysicsObject : MonoBehaviour
{
    //position of the object
    public Vector3 position;

    //velocity of the object
    public Vector3 velocity;

    //direction the object is moving, normalized
    public Vector3 direction;

    //acceleration of the object, Add to velocity each frame, reset each frame
    public Vector3 acceleration;

    //mass of the object
    [SerializeField]
    float mass;

    //toggles friction on and off
    [SerializeField]
    bool applyFriction;

    //the coefficient of friction, same for all objects in the scene
    [SerializeField]
    float coefficientOfFriction = .25f;

    //toggles gravity on and off
    [SerializeField]
    bool applyGravity;

    //toggles bounce on and off
    [SerializeField]
    bool applyBounce;

    //the strength of gravity
    private float gravityStrength = 20f;

    //the fastest speed the object can travel
    [SerializeField]
    float maximumSpeed;

    private Vector2 screenMin = new Vector2(-148, -100);

    private Vector2 screenMax = new Vector2(498, 300);

    private float timer;

    //wther or not the object will rotate
    [SerializeField]
    bool rotate = false;

    [SerializeField]
    float radius = 1f;

    public float Radius
    {
        get { return radius; }
    }

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    public float MaxSpeed
    {
        get { return maximumSpeed; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //set position to game object position
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;

        //apply gravity if toggled on
        if (applyGravity)
        {
            ApplyGravity(Vector3.down * gravityStrength);
        }

        //apply friction if toggled on
        if (applyFriction)
        {
            ApplyFriction(coefficientOfFriction);
        }

        //apply bounce after the object gets in the screen and if toggled on
        if (timer < 3)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Bounce();
        }

        //calculate velocity
        velocity += acceleration * Time.deltaTime;

        //velocity cannot increase past max speed
        velocity = Vector3.ClampMagnitude(velocity, maximumSpeed);

        //update position
        position += velocity * Time.deltaTime;

        //get current direction from velocity
        direction = velocity.normalized;

        //update gameObject position
        transform.position = position;

        //reset acceleration
        acceleration = Vector3.zero;

        //rotate object if active
        if (rotate)
        {
            RotateObject();
        }

    }

    //incorperates friction into final velocity
    private void ApplyFriction(float coeff)
    {
        //friction is opposite of velocity
        Vector3 friction = velocity * -1;
        friction.Normalize();

        //adust the strength of friction using the coefficent
        friction *= coeff;

        //adust acceleration using this
        ApplyForce(friction);
        Debug.Log(friction);
    }

    //incorperates gravity into final velocity
    private void ApplyGravity(Vector3 force)
    {
        acceleration += force;
    }

    //the objects bounce against the edges of the screen
    private void Bounce()
    {
        //check top and bottom
        if (transform.position.y <= screenMin.y)
        {
            velocity.y *= -1f;
            position.y = screenMin.y;
        }
        else if (transform.position.y >= screenMax.y)
        {
            velocity.y *= -1f;
            position.y = screenMax.y;
        }

        //check left and right
        if (transform.position.x <= screenMin.x)
        {
            velocity.x *= -1f;
            position.x = screenMin.x;
        }
        else if (transform.position.x >= screenMax.x)
        {
            velocity.x *= -1f;
            position.x = screenMax.x;
        }
    }

    //adjust acceleration with any outside influence
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    //rotates object so they are facing the velocity
    private void RotateObject()
    {
        double angle = Mathf.Atan2(velocity.y, velocity.x);
        double eulerAngle = angle * 180 / Mathf.PI;

        transform.rotation = Quaternion.Euler(0, 0, (float)eulerAngle - 90);
    }
}
