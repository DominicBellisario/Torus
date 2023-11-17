using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class SpawnManager : Singleton<SpawnManager>
{
    // (Optional) Prevent non-singleton constructor use.
    protected SpawnManager() { }

    //given to enemies when they spawn
    [SerializeField]
    HealthUpdate healthUpdate;
    [SerializeField]
    ScoreUpdate scoreUpdate;

    //spawn coords
    private int leftSpawn = -160;
    private int rightSpawn = 505;
    private int topSpawn = 310;
    private int bottomSpawn = -110;

    //need to update coll list
    [SerializeField]
    CollisionManager collisionManager;

    [SerializeField]
    PlayerInfo playerInfo;

    //-----enemy variables------

    //small asteroid prefab
    [SerializeField]
    SpriteRenderer smallAsteroidPrefab;

    //medium asteroid prefab
    [SerializeField]
    SpriteRenderer mediumAsteroidPrefab;

    //large asteroid prefab
    [SerializeField]
    SpriteRenderer largeAsteroidPrefab;

    //shoot enemy prefab
    [SerializeField]
    SpriteRenderer shootEnemyPrefab;

    //seeker enemy prefab
    [SerializeField]
    SpriteRenderer seekerEnemyPrefab;

    //enemy bullet prefab
    [SerializeField]
    SpriteRenderer enemyBulletPrefab;

    //amount of time between each wave
    [SerializeField]
    int waveTime;

    private double currentTime;
    
    //min and max for each wave
    [SerializeField]
    int minSmallAsteroids;
    [SerializeField]
    int maxSmallAsteroids;
    [SerializeField]
    int minMediumAsteroids;
    [SerializeField]
    int maxMediumAsteroids;
    [SerializeField]
    int minLargeAsteroids;
    [SerializeField]
    int maxLargeAsteroids;
    [SerializeField]
    int minShootEnemies;
    [SerializeField]
    int maxShootEnemies;
    [SerializeField]
    int minSeekerEnemies;
    [SerializeField]
    int maxSeekerEnemies;
    
    //------------------player values--------------------
    //default bullet
    [SerializeField]
    SpriteRenderer defaultBulletPrefab;

    //gets player rotation
    [SerializeField]
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //spawn first wave at the start
        currentTime = waveTime;
    }

    // Update is called once per frame
    void Update()
    {
        //increment timer
        currentTime = currentTime += Time.deltaTime;
        //spawns wave and restets timer
        if (currentTime >= waveTime)
        {
            Spawn();
            currentTime = 0;
        }
    }

    //spawns a new enemy and adds it to the list
    public SpriteRenderer SpawnSmallAsteroid(bool normalAngle)
    {
        //create a new enemy
        SpriteRenderer newSmallAsteroid = Instantiate(smallAsteroidPrefab);

        //gives the enemy references
        newSmallAsteroid.GetComponent<SmallAsteroidInfo>().SpawnManager = this;
        newSmallAsteroid.GetComponent<SmallAsteroidInfo>().HealthUpdate = healthUpdate;
        newSmallAsteroid.GetComponent<SmallAsteroidInfo>().ScoreUpdate = scoreUpdate;

        //determines what the angle of motion of the asteroid will be
        newSmallAsteroid.GetComponent<EnemyMovement>().NormalAngle = normalAngle;

        //enemy is collidable
        collisionManager.Enemies.Add(newSmallAsteroid.GetComponent<SmallAsteroidInfo>());

        return newSmallAsteroid;
    }

    public SpriteRenderer SpawnMediumAsteroid(bool normalAngle)
    {
        //create a new enemy
        SpriteRenderer newMediumAsteroid = Instantiate(mediumAsteroidPrefab);

        //gives the enemy references
        newMediumAsteroid.GetComponent<MediumAsteroidInfo>().SpawnManager = this;
        newMediumAsteroid.GetComponent<MediumAsteroidInfo>().HealthUpdate = healthUpdate;
        newMediumAsteroid.GetComponent<MediumAsteroidInfo>().ScoreUpdate = scoreUpdate;


        //determines what the angle of motion of the asteroid will be
        newMediumAsteroid.GetComponent<EnemyMovement>().NormalAngle = normalAngle;

        //enemy is collidable
        collisionManager.Enemies.Add(newMediumAsteroid.GetComponent<MediumAsteroidInfo>());

        return newMediumAsteroid;
    }

    public SpriteRenderer SpawnLargeAsteroid(bool normalAngle)
    {
        //create a new enemy
        SpriteRenderer newLargeAsteroid = Instantiate(largeAsteroidPrefab);

        //gives the enemy references
        newLargeAsteroid.GetComponent<LargeAsteroidInfo>().SpawnManager = this;
        newLargeAsteroid.GetComponent<LargeAsteroidInfo>().HealthUpdate = healthUpdate;
        newLargeAsteroid.GetComponent<LargeAsteroidInfo>().ScoreUpdate = scoreUpdate;

        //determines what the angle of motion of the asteroid will be
        newLargeAsteroid.GetComponent<EnemyMovement>().NormalAngle = normalAngle;

        //enemy is collidable
        collisionManager.Enemies.Add(newLargeAsteroid.GetComponent<LargeAsteroidInfo>());

        return newLargeAsteroid;
    }

    //spawns a new shoot enemy and adds it to the list
    public SpriteRenderer SpawnShootEnemy(bool normalAngle)
    {
        //create a new shoot enemy
        SpriteRenderer newShootEnemy = Instantiate(shootEnemyPrefab);

        //gives the enemy references
        newShootEnemy.GetComponent<ShootEnemyInfo>().SpawnManager = this;
        newShootEnemy.GetComponent<ShootEnemyInfo>().HealthUpdate = healthUpdate;
        newShootEnemy.GetComponent<ShootEnemyInfo>().ScoreUpdate = scoreUpdate;

        //determines what the angle of motion of the asteroid will be
        newShootEnemy.GetComponent<EnemyMovement>().NormalAngle = normalAngle;

        //enemy is collidable
        collisionManager.Enemies.Add(newShootEnemy.GetComponent<ShootEnemyInfo>());

        return newShootEnemy;
    }

    //spawns a new seeker enemy and adds it to the list
    public SpriteRenderer SpawnSeekerEnemy()
    {
        //create a new shoot enemy
        SpriteRenderer newSeekerEnemy = Instantiate(seekerEnemyPrefab);

        //gives the enemy references
        newSeekerEnemy.GetComponent<SeekerEnemyInfo>().SpawnManager = this;
        newSeekerEnemy.GetComponent<SeekerEnemyInfo>().HealthUpdate = healthUpdate;
        newSeekerEnemy.GetComponent<SeekerEnemyInfo>().ScoreUpdate = scoreUpdate;
        newSeekerEnemy.GetComponent<Seeker>().Target = player;

        //enemy is collidable
        collisionManager.Enemies.Add(newSeekerEnemy.GetComponent<SeekerEnemyInfo>());

        return newSeekerEnemy;
    }

    /// <summary>
    /// CHANGE TO A WAVE SYSTEM AFTER PROJECT
    /// spawns a random amount of every type of enemy
    /// </summary>
    public void Spawn()
    {
        //determines the number of enemies that will be spawned
        int smallAsteroidNum = UnityEngine.Random.Range(minSmallAsteroids, maxSmallAsteroids);
        int mediumAsteroidNum = UnityEngine.Random.Range(minMediumAsteroids, maxMediumAsteroids);
        int largeAsteroidNum = UnityEngine.Random.Range(minLargeAsteroids, maxLargeAsteroids);
        int shootEnemyNum = UnityEngine.Random.Range(minShootEnemies, maxShootEnemies);
        int seekerEnemyNum = UnityEngine.Random.Range(minSeekerEnemies, maxSeekerEnemies);

        //spawns in all small asteroids
        for (int i = 0; i < smallAsteroidNum; i++)
        {
            //spawn a new small asteroid
            SpriteRenderer newSmallAsteroid = SpawnSmallAsteroid(true);

            //set the small asteroid to its random spawn postition
            newSmallAsteroid.transform.position = DetermineSpawnPosition();
        }

        //spawns in all medium asteroids
        for (int i = 0; i < mediumAsteroidNum; i++)
        {
            //spawn a new medium asteroid
            SpriteRenderer newMediumAsteroid = SpawnMediumAsteroid(true);

            //set the medium asteroid to its random spawn postition
            newMediumAsteroid.transform.position = DetermineSpawnPosition();
        }

        //spawns in all large asteroids
        for (int i = 0; i < largeAsteroidNum; i++)
        {
            //spawn a new large asteroid
            SpriteRenderer newLargeAsteroid = SpawnLargeAsteroid(true);

            //set the large asteroid to its random spawn postition
            newLargeAsteroid.transform.position = DetermineSpawnPosition();
        }

        //spawns in all shooter enemies
        for (int i = 0; i < shootEnemyNum; i++)
        {
            //spawn a new enemy
            SpriteRenderer newShootEnemy = SpawnShootEnemy(true);

            //set the enemy to its random spawn postition
            newShootEnemy.transform.position = DetermineSpawnPosition();
        }

        //spawns in all seeker enemies
        for (int i = 0; i < seekerEnemyNum; i++)
        {
            //spawn a new enemy
            SpriteRenderer newSeekerEnemy = SpawnSeekerEnemy();

            //set the enemy to its random spawn postition
            newSeekerEnemy.transform.position = DetermineSpawnPosition();
        }
    }

    //determines a random spawn position for an enemy
    private Vector3 DetermineSpawnPosition()
    {
        Vector2 spawnPosition;

        //number that determines what side of the scene the enemy will spawn in
        float spawnSide = UnityEngine.Random.value;

        if (spawnSide < 0.25f)
        {
            spawnPosition.x = leftSpawn;
            spawnPosition.y = UnityEngine.Random.Range(bottomSpawn, topSpawn);
        }
        //enemy will spawn on the top
        else if (spawnSide < 0.50f)
        {
            spawnPosition.x = UnityEngine.Random.Range(leftSpawn, rightSpawn);
            spawnPosition.y = topSpawn;
        }
        //enemy will spawn on the right side
        else if (spawnSide < 0.75f)
        {
            spawnPosition.x = rightSpawn;
            spawnPosition.y = UnityEngine.Random.Range(bottomSpawn, topSpawn);
        }
        //enemy will spawn on the bottom
        else
        {
            spawnPosition.x = UnityEngine.Random.Range(leftSpawn, rightSpawn);
            spawnPosition.y = bottomSpawn;
        }

        return spawnPosition;
    }

    //spawns child asteroids from a point on the scene
    public void SpawnChildAsteroids(int size, int amount, Vector3 location)
    {
        //spawn the required amount
        for (int i = 0; i < amount; i++)
        {
            //spawn small asteroids at the correct location
            if (size == 1)
            {
                SpriteRenderer newSmallAsteroid = SpawnSmallAsteroid(false);
                newSmallAsteroid.transform.position = location;
            }

            //spawn medium asteroids at the correct location
            if (size == 2)
            {
                SpriteRenderer newMediumAsteroid = SpawnMediumAsteroid(false);
                newMediumAsteroid.transform.position = location;
            }
        }
    }

    //spawn a default player bullet
    public void SpawnDefaultBullet(Vector3 origin, Quaternion rotation)
    {
        //creates a new bullet
        SpriteRenderer bullet = Instantiate(defaultBulletPrefab, origin, rotation);

        //gives the bullet a spawn manager reference
        bullet.GetComponent<DefaultBulletInfo>().SpawnManager = this;

        //bullet is collidable
        collisionManager.PlayerBullets.Add(bullet.GetComponent<DefaultBulletInfo>());
    }

    //spawn enemy bullets on the shoot enemy that fired them
    public void SpawnEnemyBullet(Vector3 origin, Quaternion rotation)
    {
        SpriteRenderer bullet = Instantiate(enemyBulletPrefab, origin, rotation);

        //give the bullet a health reference
        bullet.GetComponent<EnemyBulletInfo>().HealthUpdate = healthUpdate;

        //gives the bullet a spawn manager reference
        bullet.GetComponent<EnemyBulletInfo>().SpawnManager = this;

        //bullet is collidable
        collisionManager.EnemyBullets.Add(bullet.GetComponent<EnemyBulletInfo>());
    }

    //spawn a particle effect at a location
    public void SpawnParticleEffect(ParticleSystem particleSystem, Vector3 location)
    {
        Instantiate(particleSystem, location, new Quaternion(0, 0, 0, 0));
    }
}
