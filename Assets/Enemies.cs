using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField]
    Transform player; // keep track of player's position
    [SerializeField]
    float range;
    [SerializeField]
    float moveSpeed;

    public int damage = 10;
    
    public GameObject ExplosionSound;
    AudioSource audioExplosion;
    Rigidbody2D rb2d;
    int health = 60;

    // Start is called before the first frame update
    void Start()
    {
        // ExplosionSound = FindObjectOfType<ExplosionSound>();
        rb2d = GetComponent<Rigidbody2D>();
        audioExplosion = ExplosionSound.GetComponent<AudioSource>();
    }

    // rigidbody.Addforce(new Vector2("x axis", "y axis" )) 

    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer < range)
        {
            // chase player
            ChasePlayer();
        } else {
            StopChasingPlayer();
        }
    }

    private void ChasePlayer()
    {
        if (transform.position.x < player.position.x)
        {
            transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
        else if (transform.position.x > player.position.x)
        {
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        } 
    }

    private void StopChasingPlayer()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y);
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
