using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [Header("Hit")]
    [SerializeField] Transform hitVegetablePrefab;
    [SerializeField] float hitSpawnPointOffest = 1f;
    [SerializeField][Range(0, 1)] float minTransparencyOnHit = .5f;
    [SerializeField] Vector2 knockBack = new Vector2(-1, 1);
    [SerializeField][Range(0, 5)] float dropRangeX = 2f;
    [SerializeField][Range(0, 5)] float dropRangeY = 2f;
    [SerializeField] int VegetablesDroppedPerHit = 5;
    [SerializeField] float destroyDroppedAfterTime = 2f;
    [SerializeField] float invincibilityOnHitTimer = 2f;

    private CapsuleCollider2D myCapsuleCollider2D;
    private AudioManager audioManager;
    private Rigidbody2D myRigidbody2D;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    private bool isHit = false;

    private void Awake()
    {
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        audioManager = FindObjectOfType<AudioManager>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (!isHit)
        {
            CheckForHit();
        }
        else
        {
            Flicker();
        }
    }

    private void CheckForHit()
    {
        if (myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Obstacles")))
        {
            ProcessHit();
        }
    }

    private void Flicker()
    {
        Color tmp = spriteRenderer.color;
        tmp.a = UnityEngine.Random.Range(minTransparencyOnHit, 1.0f);
        spriteRenderer.color = tmp;
    }

    private void ProcessHit()
    {
        isHit = true;
        audioManager.PlayPlayerHitSFX();
        myRigidbody2D.velocity = knockBack;
        int amountToDrop = SubVegFromInventory(VegetablesDroppedPerHit);
        DropVegetables(amountToDrop);
        StartCoroutine(ResetStateAfterHit());
    }

    private void DropVegetables(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 vegetableSpawnPoint = transform.position + Vector3.up * hitSpawnPointOffest;

            Transform droppedVegetable = Instantiate(hitVegetablePrefab, vegetableSpawnPoint, Quaternion.identity);

            //Generate a drop velocity based on a range
            float dropVelocityY = UnityEngine.Random.Range(0, dropRangeY);
            float dropVelocityX = UnityEngine.Random.Range(-dropRangeX, dropRangeX);
            Vector2 dropVelocity = new Vector2(dropVelocityX, dropVelocityY);

            //Apply the drop velocity
            droppedVegetable.GetComponent<Rigidbody2D>().velocity = dropVelocity;

            Destroy(droppedVegetable.gameObject, destroyDroppedAfterTime);
        }
    }

    //Subtracts vegetables from inventory
    //If the inventory is less than the amount to subtract returns inventory amount instead
    private int SubVegFromInventory(int amount)
    {
        if (gameManager.GetVegetableAmount() < amount)
        {
            amount = gameManager.GetVegetableAmount();
        }
        gameManager.RemoveVegetables(amount);
        return amount;
    }

    private IEnumerator ResetStateAfterHit()
    {
        yield return new WaitForSeconds(invincibilityOnHitTimer);

        //Reset hit flag
        isHit = false;

        //Change transparency back to full
        Color tmp = spriteRenderer.color;
        tmp.a = 1.0f;
        spriteRenderer.color = tmp;
    }
}
