using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool roundState;

    Rigidbody2D rb2d;
    Animator animator;
    SpriteRenderer spriteRenderer;
    SpriteRenderer bulletRenderer;

    public GameObject CoinSound;
    public GameObject GetHitSound;
    public GameObject FootstepsSound;
    public GameObject Flag;
    public GameObject GameManager;
    public GameObject PlayMenu;

    AudioSource audioCoin;
    AudioSource audioHit;
    AudioSource audioSteps;

    public int coinCount = 0;

    public float moveSpeed = 8f;
    public float jumpSpeed = 5f;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    private bool isShooting;

    [SerializeField] 
    private float shootDelay = .5f;

    [SerializeField] 
    Transform bulletSpawnPos;

    [SerializeField] 
    GameObject bullet;

    bool isGrounded = true;
    bool isWatered = false;
    bool isOnEnemy = false;
    bool isFacingLeft = false;

    // make ground check available in unity editor
    [SerializeField] 
    Transform groundCheck;

    // Start is called before the first frame update
    void Start()
    {
        // get audio
        audioCoin = CoinSound.GetComponent<AudioSource>();
        audioHit = GetHitSound.GetComponent<AudioSource>();
        audioSteps = FootstepsSound.GetComponent<AudioSource>();
        
        // set health bar
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // connect 
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Ignore collision with flag
        Physics2D.IgnoreCollision(Flag.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
    
    void Update()
    {
        // check winning/losing condition
        Result();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            // if it's shooting, stop
            if (isShooting) return;

            // shoot a bullet
            animator.Play("Jump Character Animation"); // let's pretend that he waves his hands and wabaam
            isShooting = true;

            // instantiate and shoot bullet
            GameObject aBullet = Instantiate(bullet);
            bulletRenderer = aBullet.GetComponent<SpriteRenderer>();
            if (isFacingLeft){
                bulletRenderer.flipX = true;
            } else 
            {
                bulletRenderer.flipX = false;
            }
            
            aBullet.transform.position = bulletSpawnPos.position;
            aBullet.GetComponent<Bullet>().StartShoot(isFacingLeft);

            Invoke("ResetShoot", shootDelay);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // make a line cast to detect ground underneath player
        isGrounded = (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Platform")));
        isWatered = (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Wata")));
        isOnEnemy = (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Enemy")));

        if (isOnEnemy){
            rb2d.velocity = new Vector2(-moveSpeed, jumpSpeed); // knockback effect
        }

        if (Input.GetKey("d"))
        {
            // audioSteps.Play();
            rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
            spriteRenderer.flipX = false;
            isFacingLeft = false;

            if (isGrounded)
            {
                animator.Play("Run Character Animation");
            }
        } 

        else if (Input.GetKey("a"))
        {
            audioSteps.Play();
            rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
            spriteRenderer.flipX = true;
            isFacingLeft = true;
            if (isGrounded)
            {
                animator.Play("Run Character Animation");
            }

        }

        else { 
            audioSteps.Stop();
            if (isGrounded)
            {
                
                animator.Play("Idle Character Animation");
                rb2d.velocity = new Vector2(0, 0);
            }
            else
            {
                animator.Play("Jump Character Animation");
            }
        }

        if (Input.GetKey("w") && (isGrounded || isWatered)) 
        {
            audioSteps.Stop();
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
            animator.Play("Jump Character Animation");
        } 
    }

    public void Result(){
        if (transform.position.x >= Flag.transform.position.x){
            roundState = true;
            GameManager.GetComponent<GameManager>().Determine(roundState);
            PlayMenu.GetComponent<PlayMenu>().PopUp();
            // Time.timeScale = 0f;
        } else if (currentHealth == 0 || transform.position.y < -13){
                roundState = false;
                GameManager.GetComponent<GameManager>().Determine(roundState);
                PlayMenu.GetComponent<PlayMenu>().PopUp();
                // Time.timeScale = 0f;
        }
    }

    void ResetShoot()
    {
        isShooting = false;
        animator.Play("Idle Character Animation");
    }

    void TakeDamage(int damage){
        audioHit.Play();
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if gets hit, decrease health
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Bullet" ) {
            if (collision.gameObject.tag == "Bullet" ){
                TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
                Destroy(collision.gameObject);
            } else if (collision.gameObject.tag == "Enemy" ) {
                Debug.Log("Hit");
                // if (collision.gameObject.name == "ShootingEnemy") {
                //     TakeDamage(collision.gameObject.GetComponent<EnemyShoot>().damage);
                // } else if (collision.gameObject.name == "Mace" || collision.gameObject.name == "Saw") {
                //     TakeDamage(collision.gameObject.GetComponent<Enemies>().damage);
                // }
                TakeDamage(10);

            } 
        }
        
        // if touches coin, increase coin count
        if (collision.gameObject.tag == "Coin") {
            audioCoin.Play();
            Destroy(collision.gameObject);
            coinCount += 1;
        }
    }
}
