using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour//enemies with this move at a constant speed in a straight line
{
    //min and max speed the enemy can travel
    [SerializeField]
    float minSpeed;
    [SerializeField]
    float maxSpeed;

    private float speed;
    private float xSpeed = 0;
    private float ySpeed = 0;

    //the amount the angle can deviate from the calculated angle
    [SerializeField]
    int angleRandomness;

    private float angle = 0;

    //enemy position;
    private Vector3 position;

    //min and max speed a bullet can roatate
    [SerializeField]
    int minRotSpeed;
    [SerializeField]
    int maxRotSpeed;

    private int rotationSpeed;
    private float currentRotation;

    private bool normalAngle;

    public bool NormalAngle
    {
        get { return normalAngle; } 
        set { normalAngle = value; } 
    }

    // Start is called before the first frame update
    void Start()
    {
        //sets enemy position to where it spawned
        position = transform.position;

        //gets enemy speed
        speed = UnityEngine.Random.Range(minSpeed, maxSpeed);

        //enemy spawned at the start of a wave
        if (normalAngle)
        {
            NormalSpawnAngle();
        }
        //enemy spawned from a dead parent
        else
        {
            ParentDieAngle();
        }

        //set rotation speed using min and max
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            rotationSpeed = UnityEngine.Random.Range(minRotSpeed, maxRotSpeed);
        }
        else
        {
            rotationSpeed = -UnityEngine.Random.Range(minRotSpeed, maxRotSpeed);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if wrap happened, update the object
        position = transform.position;

        //update position
        position.x += xSpeed * Time.deltaTime;
        position.y += ySpeed * Time.deltaTime;
        
        transform.position = position;

        //update rotation
        currentRotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    //enemy angle is towards center of the scene 
    private void NormalSpawnAngle()
    {
        //calcaulates angle between enemy and center of the screen
        float xDistance = 176 - transform.position.x;
        float yDistance = 100 - transform.position.y;
        angle = Mathf.Atan2(yDistance, xDistance);

        //gets a random angle using calculated angle as the normal
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            angle += UnityEngine.Random.Range(0, angleRandomness * Mathf.PI / 180);
        }
        else
        {
            angle -= UnityEngine.Random.Range(0, angleRandomness * Mathf.PI / 180);
        }

        //sets x and y speed using this angle and speed
        xSpeed = Mathf.Cos(angle) * speed;
        ySpeed = Mathf.Sin(angle) * speed;
    }

    //enemy angle could be anything
    private void ParentDieAngle()
    {
        //enemy can have any angle
        angle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

        //sets x and y speed using this angle and speed
        xSpeed = Mathf.Cos(angle) * speed;
        ySpeed = Mathf.Sin(angle) * speed;
    }
}
