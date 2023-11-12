using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletInfo : EnemyInfo
{
    protected override void Update()
    {
        //updates values for circle
        center.x = renderer.bounds.center.x;
        center.y = renderer.bounds.center.y;
    }

    public override void CollidingWithBullet(int damage)
    {
        //enemy bullet does not collide with player bullets
    }

    public override void CollidingWithPlayer()
    {
        //update player health
        healthUpdate.UpdateHealth(-damageToPlayer);

        Destroy(gameObject);
    }
}
