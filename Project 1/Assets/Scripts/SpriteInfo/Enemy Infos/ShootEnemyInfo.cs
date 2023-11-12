using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShootEnemyInfo : EnemyInfo
{
    //bullet prefab
    [SerializeField]
    SpriteRenderer enemyBulletPrefab;

    //the firing radius
    private int fireRadius = 9;

    //the locations of each gun
    private Vector3 leftGun = Vector3.zero;
    private Vector3 rightGun = Vector3.zero;
    private Vector3 upGun = Vector3.zero;
    private Vector3 downGun = Vector3.zero;

    //the angles of each gun
    private double leftAngle = Mathf.PI;
    private double rightAngle = 0;
    private double upAngle = Mathf.PI / 2;
    private double downAngle = 3 * Mathf.PI / 2;

    //time in between each shot
    [SerializeField]
    double reloadTime;
    private double timer;

    protected override void Update()
    {
        //run through normal enemy update
        base.Update();

        //increment timer
        timer += Time.deltaTime;

        //fire when timer runs out
        if (timer >= reloadTime)
        {
            timer = 0;
            //the current rotation of the enemy in radians
            float rotation = transform.eulerAngles.z * Mathf.PI / 180;

            //update gun positions
            leftGun = new Vector3(transform.position.x + fireRadius * Mathf.Cos((float)leftAngle + rotation),
                transform.position.y + fireRadius * Mathf.Sin((float)leftAngle + rotation), 0);
            rightGun = new Vector3(transform.position.x + fireRadius * Mathf.Cos((float)rightAngle + rotation),
                transform.position.y + fireRadius * Mathf.Sin((float)rightAngle + rotation), 0);
            upGun = new Vector3(transform.position.x + fireRadius * Mathf.Cos((float)upAngle + rotation),
                transform.position.y + fireRadius * Mathf.Sin((float)upAngle + rotation), 0);
            downGun = new Vector3(transform.position.x + fireRadius * Mathf.Cos((float)downAngle + rotation),
                transform.position.y + fireRadius * Mathf.Sin((float)downAngle + rotation), 0);

            //creates 4 new bullets
            spawnManager.SpawnEnemyBullet(leftGun, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90));
            spawnManager.SpawnEnemyBullet(rightGun, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 270));
            spawnManager.SpawnEnemyBullet(upGun, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z));
            spawnManager.SpawnEnemyBullet(downGun, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 180));
        }
    }
}
