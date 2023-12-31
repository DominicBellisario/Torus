using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyInfo : MonoBehaviour
{
    //for death partciles
    protected SpawnManager spawnManager;

    //for updating score upon death
    protected ScoreUpdate scoreUpdate;

    //for updating health upon collision with player
    protected HealthUpdate healthUpdate;

    //for circle collsions
    [SerializeField]
    protected float radius;

    //the amount of damage this does to the player
    [SerializeField]
    protected int damageToPlayer;

    //how many points the player gets when they destroy this
    [SerializeField]
    protected int pointsWhenDestroyed;

    [SerializeField]
    protected int health;

    //used to calculate center of circle
    [SerializeField]
    protected new SpriteRenderer renderer;

    //death particles
    [SerializeField]
    protected ParticleSystem deathParticles;

    //center of the circle
    protected Vector2 center = new Vector2();

    //chance for enemy to give player health when destroyed (1/# chance)
    [SerializeField]
    protected int healthChance;

    //--------things---------
    public Vector2 Center
    {
        get { return center; }
    }

    public float Radius
    {
        get { return radius; }
    }

    public SpawnManager SpawnManager
    {
        get { return spawnManager; }
        set { spawnManager = value; }
    }

    public HealthUpdate HealthUpdate
    {
        get { return healthUpdate; }
        set { healthUpdate = value; }
    }
    public ScoreUpdate ScoreUpdate
    {
        get { return scoreUpdate; }
        set { scoreUpdate = value; }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //updates values for circle
        center.x = renderer.bounds.center.x;
        center.y = renderer.bounds.center.y;

        //the enemy is destroyed when it runs out of health
        if (health <= 0)
        {
            ZeroHealth();
        }
    }

    //enemy loses health when a bullet hits it
    public virtual void CollidingWithBullet(int damage)
    {
        health -= damage;
    }

    //enemy dies when its health is depleted
    protected virtual void ZeroHealth()
    {
        //heal the player a bit if the chance is good
        if (UnityEngine.Random.Range(0, healthChance) == 0)
        {
            //update player health
            healthUpdate.UpdateHealth(damageToPlayer);
        }
        
        //update score
        scoreUpdate.UpdateScore(pointsWhenDestroyed);

        //spawn death particles
        if (deathParticles != null)
        {
            spawnManager.SpawnParticleEffect(deathParticles, transform.position);
        }
        

        Destroy(gameObject);
    }

    //enemy loses all of its health and hurts the player when colliding with player
    public virtual void CollidingWithPlayer()
    {
        //update player health
        healthUpdate.UpdateHealth(-damageToPlayer);

        //update score
        scoreUpdate.UpdateScore(pointsWhenDestroyed);

        Destroy(gameObject);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
