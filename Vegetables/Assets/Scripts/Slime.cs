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

    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

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
            if (!isDying)
            {
                if (hitPoints > 1)
                {
                    hitPoints--;
                    audioManager.PlaySlimeHitSFX();
                    StartCoroutine(Hit());
                }
                else
                {
                    isDying = true;
                    movementSpeed = 0f;
                    gameManager.AddPoints(points);
                    audioManager.PlaySlimeDeathSFX();
                    animator.SetTrigger("isDead");
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
