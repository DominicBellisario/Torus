using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    //used to activate boost partciles
    [SerializeField]
    PlayerParticles playerParticles;

    //position of the object in the scene
    Vector3 objectPosition = Vector3.zero;

    //velocity starts at 0
    public Vector3 objectVelocity = Vector3.zero;

    //keeps track of mouse position
    private Vector3 mousePos;

    [SerializeField]
    float accelerationPower;

    //not moving at first
    private Vector3 direction = Vector3.zero;

    private Vector3 velocity = Vector3.zero;

    //the location of the tips of the player's guns
    private int radius = 14;
    private Vector3 leftGun = Vector3.zero;
    private Vector3 rightGun = Vector3.zero;
    private double leftAngle = 11 * Math.PI / 16;
    private double rightAngle = 5 * Math.PI / 16;
    
    //the current roatation of the player
    private float playerRotation;

    //recharge time for boost, used by boost update to update boost bar
    [SerializeField]
    float boostRechargeTime;
    private float timer;

    //strength of boost
    [SerializeField]
    float boostStrength;

    //timer will not increase while the boost bar is in its animation
    private bool incrementTimer = true;

    //toggles between accelerating based on arrow keys and accelterating to get velocity to 0
    private bool zeroOut = false;

    //determines how much the player will bounce off a wall
    [SerializeField]
    int bounceAmount;

    public Vector3 LeftGun
    {
        get { return leftGun; }
    }
    public Vector3 RightGun
    {
        get { return rightGun; }
    }
    public float Timer
    {
        get { return timer/boostRechargeTime; }
    }
    public bool ZeroOut
    {
        get { return zeroOut; }
        set { zeroOut = value; }
    }
    public Vector3 Velocity
    {
        get { return velocity; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //update position if starting at a different position
        objectPosition = transform.position;

        //player can boost from the start
        timer = boostRechargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        //if wrap happened, update the object
        objectPosition = transform.position;

        //increment boost timer if recharging but not going down
        if (timer < boostRechargeTime && incrementTimer)
        {
            timer += Time.deltaTime;
        }
        //decrease timer rapidly when boost is used
        else if (!incrementTimer && timer > 0)
        {
            timer -= Time.deltaTime * 2 * boostRechargeTime * (timer + 0.1f);
        }
        //when finished, increment timer like normal
        else if (timer < 0)
        {
            timer = 0;
            incrementTimer = true;
        }

        //when not zeroing out, accelerate in the given direction
        if (!zeroOut)
        {
            Accelerate();
        }
        //otherwize, zero out
        else
        {
            TryZeroOut();
        }

        //if player is outside the bounds of the scene, push them back in
        StayInBounds();

        //update position with the speed
        objectPosition.x += velocity.x * Time.deltaTime;
        objectPosition.y += velocity.y * Time.deltaTime;

        //no more changes to velocity/position, update the position in game
        transform.position = objectPosition;

        //gets the x and y of the mouse
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //calculates player rotation with the mouse
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        //update player and gun barrel rotation
        UpdatePlayerAndGunRotation();
    }

    /// <summary>
    /// gets a new direction vector from inputController
    /// </summary>w
    /// <param name="newDirection"></param>
    public void SetDirection(Vector2 newDirection)
    {
        // direction vector must be normal and not null
        if (newDirection != null)
        {
            direction = newDirection.normalized;
        }
    }

    //change player velocity in a direction
    public void Boost()
    {
        //only work if off cooldown and holding down WASD
        if (timer >= boostRechargeTime && direction != Vector3.zero)
        {
            incrementTimer = false;

            //update velocity so player moves
            velocity += direction * boostStrength;

            //turn on boost particles
            playerParticles.BoostEffect();
        }
    }

    //acccelerate to try to make velocity = 0
    private void TryZeroOut()
    {
        //get the angle of negative velocity
        double angle = 0;
        if (velocity.x != 0)
        {
            angle = Math.Atan2(-velocity.y, -velocity.x);
        }
        

        //accelerate the player opposite of the velocity so that both x and y
        //velocity = 0 at the same time
        velocity.x += accelerationPower * Time.deltaTime * (float)Math.Cos(angle);
        velocity.y += accelerationPower * Time.deltaTime * (float)Math.Sin(angle);
    }

    //accelerate in the current direction
    private void Accelerate()
    {
        //left acceleration
        if (direction.x > 0)
        {
            velocity.x += accelerationPower * Time.deltaTime;
        }
        //right acceleration
        else if (direction.x < 0)
        {
            velocity.x -= accelerationPower * Time.deltaTime;
        }
        //up acceleration
        if (direction.y > 0)
        {
            velocity.y += accelerationPower * Time.deltaTime;
        }
        //down acceleration
        else if (direction.y < 0)
        {
            velocity.y -= accelerationPower * Time.deltaTime;
        }
    }

    //bounces the player back in bounds if nessisary
    private void StayInBounds()
    {
        int leftWrap = -160;
        int rightWrap = 505;
        int topWrap = 310;
        int bottomWrap = -110;

        //left side
        if (objectPosition.x < leftWrap)
        {
            objectPosition = new Vector3(leftWrap + 1, objectPosition.y, objectPosition.z);
            velocity.x = -velocity.x / bounceAmount;
        }
        //right side
        else if (objectPosition.x > rightWrap)
        {
            objectPosition = new Vector3(rightWrap -1, objectPosition.y, objectPosition.z);
            velocity.x = -velocity.x / bounceAmount;
        }
        //bottom
        else if (objectPosition.y > topWrap)
        {
            objectPosition = new Vector3(objectPosition.x, topWrap - 1, objectPosition.z);
            velocity.y = -velocity.y / bounceAmount;
        }
        //top
        else if (objectPosition.y < bottomWrap)
        {
            objectPosition = new Vector3(objectPosition.x, bottomWrap + 1, objectPosition.z);
            velocity.y = -velocity.y / bounceAmount;
        }
    }


    private void UpdatePlayerAndGunRotation()
    {
        //update player rotation
        if (transform.eulerAngles.z <= 180f)
        {
            playerRotation = transform.eulerAngles.z;
        }
        else
        {
            playerRotation = transform.eulerAngles.z - 360f;
        }

        //convert to radians
        playerRotation *= (float)Math.PI / 180;

        //update left and right gun position
        leftGun = new Vector3(transform.position.x + radius * Mathf.Cos((float)leftAngle + playerRotation),
            transform.position.y + radius * Mathf.Sin((float)leftAngle + playerRotation), 0);
        rightGun = new Vector3(transform.position.x + radius * Mathf.Cos((float)rightAngle + playerRotation),
            transform.position.y + radius * Mathf.Sin((float)rightAngle + playerRotation), 0);
    }
}
