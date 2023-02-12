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

    void Update()
    {
        if (PauseControl.isPaused) { return; }
        move();
        flip();
        Climb();
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
            //If the player pushes up while near a ladder climbing should start
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
        //Dont let the player move if they are picking
        if(isPicking){return;}
        
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

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if colliding with ground set isJumping to false
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Platforms")))
        {
            myAnimator.SetBool("isJumping", false);
        }
    }

    public bool GetIsClimbing()
    {
        return isClimbing;
    }
}
