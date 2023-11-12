using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBulletInfo : MonoBehaviour
{
    //the damage the bullet does when it collides
    [SerializeField]
    protected int damage;

    //for circle collsions
    [SerializeField]
    protected float radius;

    //used to calculate center of circle
    [SerializeField]
    protected new SpriteRenderer renderer;

    //center of the circle
    protected Vector2 center = new Vector2();

    //particle effect the bullet has when it is destroyed
    protected ParticleSystem deathParticles;

    //--------things---------
    public Vector2 Center
    {
        get { return center; }
    }

    public float Radius
    {
        get { return radius; }
    }

    public int Damage
    {
        get { return damage; }
    }

    //spawn manager gives the value
    public ParticleSystem DeathParticles
    {
        get { return deathParticles; }
        set { deathParticles = value; }
    }

    // Update is called once per frame
    protected void Update()
    {
        //updates values for circle
        center.x = renderer.bounds.center.x;
        center.y = renderer.bounds.center.y;
    }

    //what happens to the bullet when it collides with an enemy
    public void CollidingWithEnemy()
    {
        
        Destroy(gameObject);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
