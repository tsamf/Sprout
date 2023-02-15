using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlantedVegetable : MonoBehaviour
{

    [SerializeField] VegetableSO vegetableInfo;
    [SerializeField] TextMeshProUGUI vegetableCountText;

    [Header("Pick Parameters")]
    [SerializeField] float pickSpeed = 1f;
    [SerializeField] float moveDistance = 1f;
    [SerializeField] float displayTextTime = 1f;
    private Vector3 calculatedPickDestination;

    private BoxCollider2D myBoxCollider2D;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    private bool gettingPicked = false;
    private bool isPicked = false;

    private void Awake()
    {
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerPick.OnPick += StartPick;
    }
    private void OnDisable()
    {
        PlayerPick.OnPick -= StartPick;
    }

    private void Start()
    {
        //Calculate the pick destination and store it for later
        calculatedPickDestination = transform.position + Vector3.up * moveDistance;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        //Vegetable has entered the getting picked state and starts moving upwards
        if (gettingPicked)
        {
            //Max distance to move between updates
            float maxDistance = pickSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, calculatedPickDestination, maxDistance);
        }

        //Once the Vegetable has reached its destination mark as picked and display Collection text
        if (transform.position == calculatedPickDestination && !isPicked)
        {
            isPicked = true;
            spriteRenderer.enabled = false;
            StartCoroutine(displayPickedText());
        }
    }

    IEnumerator displayPickedText()
    {
        gameManager.AddVegetables(vegetableInfo.GetAmount());
        vegetableCountText.text = "+" + vegetableInfo.GetAmount().ToString();
        vegetableCountText.enabled = true;
        yield return new WaitForSeconds(displayTextTime);
        Destroy(gameObject);
    }

    //Once a pick even is recieved this method is called to check that it is this vegetable the player is touching
    //If it is the sprite is switched to the vegetable type and the gettingPicked flag is set
    private void StartPick()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            spriteRenderer.sprite = vegetableInfo.GetVegetableSprite();
            gettingPicked = true;
        }
    }
}
