using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class BulletMovement : MonoBehaviour
{
    //speed of the bullet
    [SerializeField]
    float speed;
    private float xSpeed;
    private float ySpeed;

    //bullet position
    private Vector3 position = Vector3.zero;

    //timer
    [SerializeField]
    double deathTimer;

    private double timer;

    // Start is called before the first frame update
    void Start()
    {
        //sets position
        position = transform.position;

        //sets x and y speed using bullet rotation and speed
        xSpeed = Mathf.Cos((transform.eulerAngles.z + 90) * Mathf.PI / 180) * speed;
        ySpeed = Mathf.Sin((transform.eulerAngles.z + 90) * Mathf.PI / 180) * speed;
    }

    // Update is called once per frame
    void Update()
    {
        //update x and y pos
        position.x += xSpeed * Time.deltaTime;
        position.y += ySpeed * Time.deltaTime;

        transform.position = position;

        //increment timer
        timer += Time.deltaTime;

        if (timer > deathTimer)
        {
            Destroy(gameObject);
        }
    }
}
