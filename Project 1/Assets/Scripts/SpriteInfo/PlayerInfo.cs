using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AllWeapons
{
    _default
}
public class PlayerInfo : MonoBehaviour
{
    //for bullet spawning
    [SerializeField]
    SpawnManager spawnManager;
    [SerializeField]
    MovementController movementController;

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

    
    //-----------weapon stuff--------------

    //the gun in slot 1
    private string gunInSlot1;

    //the gun in slot 2
    private string gunInSlot2;

    //keeps track if the player is shooting or not
    private bool isShooting;

    //keeps track of time for shooting
    private float shootTimer;

    //toggles between left and right gun firing
    private bool leftGun;

    //toggles between slot 1 and 2
    private bool slot1 = true;

    //"default": weak, medium firerate, medium bullet speed, infinite ammo, goes in slot 1 when player has no guns
    //default weapon shoot speed
    [SerializeField]
    float defaultCooldown;

    //"shotgun" medium, close range, slow firing speed, ### ammo
    [SerializeField]
    float shotgunCooldown;

    //number of times the shotgun can fire
    [SerializeField]
    int shotgunAmmo;

    //--------properties---------

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

    //used by collision manager to toggle player collisions
    public bool IsInvulnerable
    {
        get { return isInvulnerable; }
    }

    //toggled by input manager when left click is down or up
    public bool IsShooting
    {
        get { return isShooting; }
        set { isShooting = value; }
    }

    void Start()
    {
        //player can be hit off rip
        invulnTimer = invulnTime;

        //player able to shoot at the start
        shootTimer = defaultCooldown;
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

        //increment shoot timer wether they want to shoot or not (shot is always cooling down)
        shootTimer = shootTimer += Time.deltaTime;

        //if left click is held down while shot is off cooldown, try to shoot
        if (isShooting && shootTimer >= defaultCooldown)
        {
            //reset shoot timer
            shootTimer = 0;

            if (leftGun)
            {
                //fire a bullet from the left gun
                spawnManager.SpawnDefaultBullet(movementController.LeftGun, transform.rotation);
                leftGun = false;
            }
            else
            {
                //fire a bullet from the right gun
                spawnManager.SpawnDefaultBullet(movementController.RightGun, transform.rotation);
                leftGun = true;
            }
            
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

    //update the player's current slot with the weapon the player collided with 
    public void UpdateLoadout(string weaponName)
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

