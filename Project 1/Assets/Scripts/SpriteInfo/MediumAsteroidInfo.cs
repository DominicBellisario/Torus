using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumAsteroidInfo : EnemyInfo
{
    //the amount of children it spawns when it dies
    [SerializeField]
    int numberOfChildren;

    //dying spawns smaller asteroids
    protected override void ZeroHealth()
    {
        spawnManager.SpawnChildAsteroids(1, numberOfChildren, transform.position);
        base.ZeroHealth();
    }

    //hitting the player spawns smaller asteroids
    public override void CollidingWithPlayer()
    {
        spawnManager.SpawnChildAsteroids(1, numberOfChildren, transform.position);
        base.CollidingWithPlayer();
    }
}
