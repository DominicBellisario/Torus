using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    //for circle collsions
    [SerializeField]
    float radius;

    private bool collidingWithEnemy = false;
    private bool collidingWithEnemyBullet = false;

    //used to calculate center of circle
    [SerializeField]
    new SpriteRenderer renderer;

    //center of the circle
    private Vector2 center = new Vector2();

    //the player's health starts at 100%
    private int health = 100;

    //the time the player is invulnerable for when colliding with an enemy
    [SerializeField]
    float invulnTime;
    private float invulnTimer;
    private bool isInvulnerable;

    //the speed the player flashes while invulnerable
    [SerializeField]
    float flashSpeed;
    private float flashTimer;
    private bool toggleColor = false;

    [SerializeField]
    float gunCooldown;

    //--------things---------

    //used for circle collisions
    public Vector2 Center
    {
        get { return center; }
    }
    
    //used by collision manager
    public bool CollidingWithEnemy
    {
        set { collidingWithEnemy = value; }
    }

    //used by collision manager
    public bool CollidingWithEnemyBullet
    {
        set { collidingWithEnemyBullet = value; }
    }

    //used by collision manager
    public float Radius
    {
        get { return radius; }
    }

    //used by collision manager and to update the HUD
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public float GunCooldown
    {
        get { return gunCooldown; }
    }

    //used by collision manager to toggle player collisions
    public bool IsInvulnerable
    {
        get { return isInvulnerable; }
    }

    void Start()
    {
        //player can be hit off rip
        invulnTimer = invulnTime;
    }

    // Update is called once per frame
    void Update()
    {
        //updates values for circle
        center.x = renderer.bounds.center.x;
        center.y = renderer.bounds.center.y;

        //when colliding with enemy
        if (collidingWithEnemy)
        {
            collidingWithEnemy = false;
            //player is invulnerable for a time
            invulnTimer = 0;
        }
        //when colliding with enemy bullet
        else if (collidingWithEnemyBullet)
        {
            collidingWithEnemyBullet = false;
            //player is invulnerable for a time
            invulnTimer = 0;
        }

        //when the player dies, slow down time and remove the player from the scene
        if (health <= 0)
        {
            Time.timeScale = 0.2f;
            Destroy(gameObject);
        }

        //while invulnerable
        if (invulnTimer < invulnTime)
        {
            invulnTimer += Time.deltaTime;
            isInvulnerable = true;
            Flash(Color.red);
        }
        else
        {
            isInvulnerable = false;
            renderer.color = Color.white;
        }
    }

    //alternate between two different colors
    private void Flash(Color color1)
    {
        flashTimer += Time.deltaTime;
        if (flashTimer >= flashSpeed)
        {
            flashTimer = 0;
            toggleColor = !toggleColor;
        }

        if (toggleColor)
        {
            renderer.color = color1;
        }
        else
        {
            renderer.color = Color.clear;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

