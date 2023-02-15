using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedVegetable : MonoBehaviour
{

    [SerializeField] float travelSpeed = 10f;
    [SerializeField] float travelDelay = .2f;
    [SerializeField] int vegetablePoints = 5;

    private GameObject basket;
    private GameManager gameManager;
    private AudioManager audioManager;
    private bool isTraveling;

    //Once the object has spawned start the timer before traveling to the basket
    private void Start() {
        audioManager = FindObjectOfType<AudioManager>();
        basket = FindObjectOfType<Basket>().gameObject;
        gameManager = FindObjectOfType<GameManager>();

        StartCoroutine(TriggerTravel());
    }

    //Wait for a given ammount of time before starting to travel towards the basket
    IEnumerator TriggerTravel()
    {
        yield return new WaitForSeconds(travelDelay);
        isTraveling = true;
    }

    private void Update(){
        //Once the vegetable has dropped from the play and some time has passed move towards the basket
        if(isTraveling)
        {   
            float travelDelta = travelSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, basket.transform.position, travelDelta);

            //Once the Vegetable reaches the basket  destroy it
            if(transform.position == basket.transform.position)
            {
                audioManager.PlayVegetableTurnedInSFX();
                gameManager.AddPoints(vegetablePoints);
                Destroy(gameObject);
            }
        }
    }
}
