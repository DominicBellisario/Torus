using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletInfo : MonoBehaviour
{
    //the damage the bullet does when it collides
    [SerializeField]
    int damage;

    //for circle collsions
    [SerializeField]
    float radius;

    //wether or not the bullet is hitting an enemy
    private bool collidingWithEnemy = false;

    //used to calculate center of circle
    [SerializeField]
    new SpriteRenderer renderer;

    //center of the circle
    private Vector2 center = new Vector2();

    //--------things---------
    public Vector2 Center
    {
        get { return center; }
    }

    public bool CollidingWithEnemy
    {
        set { collidingWithEnemy = value; }
    }

    public float Radius
    {
        get { return radius; }
    }

    public int Damage
    {
        get { return damage; }
    }

    // Update is called once per frame
    void Update()
    {
        //updates values for circle
        center.x = renderer.bounds.center.x;
        center.y = renderer.bounds.center.y;

        //bullet is destroyed when it hits an enemy
        if (collidingWithEnemy)
        {
            collidingWithEnemy = false;
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
