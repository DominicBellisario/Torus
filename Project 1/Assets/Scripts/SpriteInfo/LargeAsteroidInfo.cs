using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.PlayerLoop.EarlyUpdate;

public class LargeAsteroidInfo : EnemyInfo
{
    //the amount of children it spawns when it dies
    [SerializeField]
    int numberOfChildren;

    //dying spawns smaller asteroids
    protected override void ZeroHealth()
    {
        spawnManager.SpawnChildAsteroids(2, numberOfChildren, transform.position);
        base.ZeroHealth();
    }

    //hitting the player spawns smaller asteroids
    public override void CollidingWithPlayer()
    {
        spawnManager.SpawnChildAsteroids(2, numberOfChildren, transform.position);
        base.CollidingWithPlayer();
    }
}
