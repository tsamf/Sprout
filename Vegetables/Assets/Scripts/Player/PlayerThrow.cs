using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrow : MonoBehaviour
{
    [Header("Projectiles")]
    [SerializeField] Transform thrownVegetablePrefab;
    [SerializeField] Vector2 throwSpeed = new Vector2(2, 2);
    [SerializeField] float thrownSpawnPointOffest = 1f;
    [SerializeField] float destroyThrownAfterTime = 2f;
    [SerializeField] float throwCooldownTimer = .1f;

    private PlayerMovement playerMovement;
    private Animator animator;
    private GameManager gameManager;
    private AudioManager audioManager;
    private Rigidbody2D myRigidbody2D;

    private bool isThrowing = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnFire(InputValue value)
    {
        //If the game is paused don't do anything
        if (PauseControl.isPaused) { return; }
        if (EndOfLevelMenu.isLevelOver) { return; }
        //If the player is on the ladder don't do anything
        if (playerMovement.GetIsClimbing()) { return; }

        if (value.isPressed && !isThrowing)
        {
            ActivateThrow();
        }
    }

    private void ActivateThrow()
    {
        animator.SetTrigger("isThrowing");
        isThrowing = true;

        if (gameManager.GetVegetableAmount() <= 0)
        {
            ThrowEmpty();
        }
        else
        {
            ThrowVegetable();
        }

        StartCoroutine(ResetThrowState());
    }

    private void ThrowVegetable()
    {
        audioManager.PlayPlayerThrowSFX();
        gameManager.RemoveVegetables(1);
        SpawnVegetable();
    }

    private void SpawnVegetable()
    {
        Vector3 vegetableSpawnPoint = transform.position + Vector3.up * thrownSpawnPointOffest;
        Transform weapon = Instantiate(thrownVegetablePrefab, vegetableSpawnPoint, Quaternion.identity);
        //Using the players speed to apply further distance to throw kind of wonky needs to be reworked.
        //Vector2 throwDistance = new Vector2(-transform.localScale.x * (Mathf.Abs(myRigidbody2D.velocity.x) + throwSpeed.x), myRigidbody2D.velocity.y + throwSpeed.y);
        Vector2 throwDistance = new Vector2(throwSpeed.x * -transform.localScale.x, throwSpeed.y); 
        weapon.GetComponent<Rigidbody2D>().velocity = throwDistance;
        Destroy(weapon.gameObject, destroyThrownAfterTime);
    }

    private void ThrowEmpty()
    {
        audioManager.PlayplayerThrowEmptySFX();
    }

    IEnumerator ResetThrowState()
    {
        yield return new WaitForSeconds(throwCooldownTimer);
        isThrowing = false;
    }
}
