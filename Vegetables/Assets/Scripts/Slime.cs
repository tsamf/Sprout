using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    [SerializeField] float movementSpeed = 10f;
    [SerializeField] int points = 100;
    [SerializeField] int hitPoints = 2;
    [SerializeField] float slimeDeathTimer = 1f;
    [SerializeField] float slimeHitTimer = .1f;
    [SerializeField] Color colorOnHit = Color.red;

    private Rigidbody2D myRigidbody2D;
    private GameManager gameManager;
    private AudioManager audioManager;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isDying = false;
    private Color originalColor;
    private BoxCollider2D boxCollider2D;
    private CircleCollider2D circleCollider2D;

    void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody2D.velocity = new Vector2(movementSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("ThrowVegetable"))
        {
            //If the slime is not in a dying state process the hit
            if (!isDying)
            {
                //if hitpoints higher than 1 decrement and play hit sound
                if (hitPoints > 1)
                {
                    hitPoints--;
                    audioManager.PlaySlimeHitSFX();
                    StartCoroutine(Hit());
                }
                //Slime has reached 0 hit points start death process
                else
                {
                    isDying = true;
                    movementSpeed = 0f;
                    gameManager.AddPoints(points);
                    gameManager.IncrementEnemiesKilled();
                    audioManager.PlaySlimeDeathSFX();
                    animator.SetTrigger("isDead");
                    //Prevents player from taking damage while death animation is playing
                    boxCollider2D.enabled = false;
                    circleCollider2D.enabled= false;
                    StartCoroutine(Die());
                }
            }
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(slimeDeathTimer);
        Destroy(gameObject);
    }

    IEnumerator Hit()
    {
        Color temp = spriteRenderer.color;
        spriteRenderer.color = colorOnHit;
        yield return new WaitForSeconds(slimeHitTimer);
        spriteRenderer.color = originalColor;
    }

    //When the enemy collider leaves the platforms plane it turns around and pats the other direction
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platforms"))
        {
            movementSpeed = -movementSpeed;
            FlipEnemyFacing();
        }
    }

    private void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-transform.localScale.x, 1f);
    }
}
