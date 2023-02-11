using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{


    [Header("Movement Speed")]
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float climbSpeed = 10f;

    [Header("Timers")]
    [SerializeField] float deathTimer = 1f;
    [SerializeField] float pickingTimer = .5f;
    [SerializeField] float throwCooldownTimer = .1f;

    [Header("Projectiles")]
    [SerializeField] Transform thrownVegetablePrefab;
    [SerializeField] Vector2 throwSpeed = new Vector2(2, 2);
    [SerializeField] float thrownSpawnPointOffest = 0f;
    [SerializeField] float destroyThrownAfterTime = 2f;

    private Rigidbody2D myRigidbody2D;
    private float myGravityScaleAtStart;

    private Vector2 moveInput;
    private Animator myAnimator;
    private BoxCollider2D myBoxCollider2D;
    private CapsuleCollider2D myCapsuleCollider2D;
    private AudioManager audioManager;
    private GameManager gameManager;


    private bool isClimbing = false;
    private bool isPicking = false;
    private bool isThrowing = false;

    public static event Action OnPick;

    private void OnEnable()
    {
        OnPick += pickVegetable;
    }

    private void OnDisable()
    {
        OnPick -= pickVegetable;
    }

    void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myGravityScaleAtStart = myRigidbody2D.gravityScale;
        myAnimator = GetComponent<Animator>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        audioManager = FindObjectOfType<AudioManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (PauseControl.isPaused) { return; }
        move();
        flip();
        Climb();
        Pick();
    }

    private void Pick()
    {
        if (myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Vegetables")))
        {
            if (moveInput.y < 0 && !isPicking)
            {
                OnPick?.Invoke();
            }
        }
    }

    private void pickVegetable()
    {
        myAnimator.SetTrigger("isPicking");
        audioManager.PlayPlayerPickSFX();
        isPicking = true;
        StartCoroutine(resetPicking());
    }


    IEnumerator resetPicking()
    {
        yield return new WaitForSeconds(pickingTimer);
        isPicking = false;
    }


    private void Climb()
    {
        //Got off a ladder  
        if (!myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders")) && isClimbing)
        {
            myRigidbody2D.gravityScale = myGravityScaleAtStart;
            myAnimator.enabled = true;
            isClimbing = false;

            //Turn off climbing animation
            myAnimator.SetBool("isClimbing", false);

            if (!myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platforms")))
            {
                //If the player isnt on the ground turn on jumping animation
                myAnimator.SetBool("isJumping", true);
            }
        }

        //Near a ladder
        if (myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            //If the player pushes up while touching a ladder climbing should start
            if (moveInput.y > 0)
            {
                isClimbing = true;
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isJumping", false);
            }

            // The player is currently on the ladder
            if (isClimbing)
            {
                Vector2 climbVelocity = new Vector2(myRigidbody2D.velocity.x, moveInput.y * climbSpeed);
                myRigidbody2D.velocity = climbVelocity;
                myRigidbody2D.gravityScale = 0;

                if (!myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platforms")))
                {
                    myAnimator.SetBool("isClimbing", true);
                }
                else
                {
                    if (moveInput.y < 0)
                    {
                        isClimbing = false;
                        myRigidbody2D.gravityScale = myGravityScaleAtStart;
                        myAnimator.enabled = true;
                        myAnimator.SetBool("isClimbing", false);
                    }
                }

                //Check if the player has vertical movement if not pause the climbing animation
                bool playerHasVerticalSpeed = Math.Abs(myRigidbody2D.velocity.y) > Mathf.Epsilon;
                myAnimator.enabled = playerHasVerticalSpeed;
            }
        }
    }


    private void move()
    {
        myRigidbody2D.velocity = new Vector2(moveInput.x * runSpeed, myRigidbody2D.velocity.y);

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(-Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }

    private void flip()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnFire(InputValue value)
    {
        //If the game is paused don't do anything
        if (PauseControl.isPaused) { return; }
        //Do not do anything if the player is on the ladder
        if(isClimbing) {return;}

        if (value.isPressed && !isThrowing)
        {
            myAnimator.SetTrigger("isThrowing");
            isThrowing = true;
            StartCoroutine(throwCooldown());

            //The player doesnt have vegetables
            if (gameManager.GetVegetableAmount() <= 0)
            {
                audioManager.PlayplayerThrowEmptySFX();
            }
            //The player has Vegetables
            else
            {
                audioManager.PlayPlayerThrowSFX();
                gameManager.RemoveVegetables(1);

                //Spawn a vegitable and throw it
                Vector3 vegetableSpawnPoint = transform.position + Vector3.up * thrownSpawnPointOffest;
                Transform weapon = Instantiate(thrownVegetablePrefab, vegetableSpawnPoint, Quaternion.identity);
                Vector2 throwDistance = new Vector2(-transform.localScale.x * (Mathf.Abs(myRigidbody2D.velocity.x) + throwSpeed.x), myRigidbody2D.velocity.y + throwSpeed.y);
                weapon.GetComponent<Rigidbody2D>().velocity = throwDistance;
                Destroy(weapon.gameObject, destroyThrownAfterTime);
            }
        }
    }

    IEnumerator throwCooldown()
    {
        yield return new WaitForSeconds(throwCooldownTimer);
        isThrowing = false;
    }

    private void OnJump(InputValue value)
    {
        //If the game is paused don't do anything
        if (PauseControl.isPaused) { return; }

        //If the player is not on the ground don't jump        
        if (!myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platforms"))) { return; }

        if (value.isPressed)
        {
            myRigidbody2D.velocity += new Vector2(0, jumpSpeed);
            myAnimator.SetBool("isJumping", true);
            audioManager.PlayPlayerJumpSFX();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if colliding with ground set isJumping to false
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platforms")))
        {
            myAnimator.SetBool("isJumping", false);
        }
    }
}
