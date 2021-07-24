using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    GameObject Shooter;

    float bulletSpeed = 20f;
    public int damage = 10;
    float bulletTime = 1.5f;

    public void StartShoot(bool isFacingLeft)
    {
        
        if(!isFacingLeft)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
        } else if (isFacingLeft)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletSpeed, 0);
        }

        Destroy(gameObject, bulletTime);
    }

     void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Platform")
            {
                Destroy(gameObject);
            }
        }
    
    public void AvoidCollision (GameObject shooter){
        Physics2D.IgnoreCollision(shooter.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
}
