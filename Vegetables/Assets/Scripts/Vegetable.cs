using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour
{

    [SerializeField] VegetableSO veggieInfo;

    [Header("Pick Parameters")]
    [SerializeField] float pickSpeed = 1f;
    [SerializeField] float moveDistance = 1f;
    private Vector3 calculatedPickDestination;

    private BoxCollider2D myBoxCollider2D;
    private SpriteRenderer spriteRenderer;
    private bool isPicked = false;



    private void OnEnable()
    {
        PlayerMovement.OnPick += IGotPicked;
    }
    private void OnDisable()
    {
        PlayerMovement.OnPick -= IGotPicked;
    }

    private void Awake()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //Calculate the pick destination and store it for later
        calculatedPickDestination = transform.position + Vector3.up * moveDistance;
    }

    private void Update()
    {
        if (isPicked)
        {
            //Max distance to move between updates
            float maxDistance = pickSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, calculatedPickDestination, maxDistance);

            if (transform.position == calculatedPickDestination)
            {
                Destroy(gameObject);
            }
        }
    }

    private void IGotPicked()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            spriteRenderer.sprite = veggieInfo.GetVegetableSprite();
            isPicked = true;
        }
    }
}
