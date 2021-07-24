using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField]
    Transform player; // keep track of player's position

    [SerializeField]
    GameObject EnemyBullet; // keep track of player's position

    [SerializeField] 
    GameObject EnemyBulletSpawn;

    float range = 8;
    float moveSpeed = 4;
    float shootDelay = .8f;

    public int damage = 10;
    SpriteRenderer bulletRenderer;
    public GameObject ExplosionSound;
    AudioSource audioExplosion;
    Rigidbody2D rb2d;
    int health = 80;

    bool isShooting = false;
    bool isFacingLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        // ExplosionSound = FindObjectOfType<ExplosionSound>();
        rb2d = GetComponent<Rigidbody2D>();
        audioExplosion = ExplosionSound.GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer < range)
        {
            // chase player
            ChasePlayer();
            ShootPlayer();
        } else {
            StopChasingPlayer();
        }
    }

    private void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            isFacingLeft = false;
        }
        else if (transform.position.x > player.position.x)
        {
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
            isFacingLeft = true;
        } 
    }

    private void StopChasingPlayer()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y);
    }

    private void ShootPlayer()
    {
        if (isShooting) return;

        isShooting = true;
        GameObject aBullet = Instantiate(EnemyBullet);
        bulletRenderer = aBullet.GetComponent<SpriteRenderer>();
        if (isFacingLeft){
            bulletRenderer.flipX = false;
        } else 
        {
            bulletRenderer.flipX = true;
        }
        aBullet.transform.position = EnemyBulletSpawn.transform.position;
        aBullet.GetComponent<Bullet>().StartShoot(isFacingLeft);
        aBullet.GetComponent<Bullet>().AvoidCollision(gameObject);
        Invoke("ResetShoot", shootDelay);
    }

    void ResetShoot()
    {
        isShooting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // FindObjectOfType<ExplosionSound>.GetComponent<AudioSource>().Play();
            audioExplosion.Play();
            health -= 20;
            Destroy(collision.gameObject);

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
