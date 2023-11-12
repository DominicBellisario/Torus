using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    //used to update score when certain collisions happen
    [SerializeField]
    ScoreUpdate scoreUpdate;

    //used to update health when certain collisions happen
    [SerializeField]
    HealthUpdate healthUpdate;

    [SerializeField]
    SpawnManager spawnManager;

    //--------COLLIDABLE OBJECTS--------
    //player
    [SerializeField]
    PlayerInfo player;

    //every player bullet
    private List <PlayerBulletInfo> playerBullets = new List<PlayerBulletInfo>();
    public List<PlayerBulletInfo> PlayerBullets
    {
        get { return playerBullets; }
        set { playerBullets = value; }
    }

    //every enemy bullet
    private List<EnemyBulletInfo> enemyBullets = new List<EnemyBulletInfo>();
    public List<EnemyBulletInfo> EnemyBullets
    {
        get { return enemyBullets; }
        set { enemyBullets = value; }
    }

    //every enemy
    public List<EnemyInfo> enemies = new List<EnemyInfo>();
    public List<EnemyInfo> Enemies
    {
        get { return enemies; }
        set { enemies = value; }
    }

    // Update is called once per frame
    void Update()
    {
        //-----------COLLISION LOOPS--------------

        //loops through every enemy
        foreach (var enemy in enemies)
        {
            //loops through every player bullet
            foreach (var playerBullet in playerBullets)
            {
                //if a player bullet collides with an enemy
                if (CircleCheck(enemy.Center, enemy.Radius, playerBullet.Center, playerBullet.Radius))
                {
                    //notifies spriteInfo that these objects are colliding
                    playerBullet.CollidingWithEnemy();
                    enemy.CollidingWithBullet(playerBullet.Damage);

                    playerBullets.Remove(playerBullet);

                    //ends loop
                    return;
                }
            }
            //when enemy collides with player while not invulnerable
            if (CircleCheck(enemy.Center, enemy.Radius, player.Center, player.Radius) && !player.IsInvulnerable)
            {
                //notifies spriteInfo that these objects are colliding
                player.CollidingWithEnemy = true;
                enemy.CollidingWithPlayer();

                //ends loop
                return;
            }
        }

        //loops through every enemy bullet
        foreach (var enemyBullet in enemyBullets)
        {
            //if an enemy bullet collides with the player while not invulnerable
            if (CircleCheck(enemyBullet.Center, enemyBullet.Radius, player.Center, player.Radius) && !player.IsInvulnerable)
            {
                //notifies spriteInfo that these objects are colliding
                player.CollidingWithEnemyBullet = true;
                enemyBullet.CollidingWithPlayer();
                

                //ends loop
                return;
            }
        }

        //------------------CLEAN UP LOOPS-----------------
        //loops through each list and removes any objects that have been destroyed from their lists

        foreach (var playerBullet in playerBullets)
        {
            if (playerBullet == null)
            {
                playerBullets.Remove(playerBullet);
                return;
            }
        }

        foreach (var enemy in enemies)
        {
            if (enemy == null)
            {
                enemies.Remove(enemy);
                return;
            }
        }

        foreach (var enemyBullet in enemyBullets)
        {
            if (enemyBullet == null)
            {
                enemyBullets.Remove(enemyBullet);
                return;
            }
        }

        //disables the collision manager when player runs out of lives, disabling collisions
        if (player == null)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// checks two circles to see if they collide using pythag
    /// </summary>
    /// <param name="spriteA"></param>
    /// <param name="spriteB"></param>
    /// <returns></returns>
    private bool CircleCheck(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
    {
        if (Mathf.Sqrt(Mathf.Pow(centerA.x - centerB.x, 2) + Mathf.Pow(centerA.y - centerB.y, 2)) <=
            radiusA + radiusB)
        { 
            return true;
        }
        return false;
    }
}
