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
    private bool isTraveling;

    void Awake()
    {
        basket = FindObjectOfType<Basket>().gameObject;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start() {
        StartCoroutine(TriggerTravel());
    }

    IEnumerator TriggerTravel()
    {
        yield return new WaitForSeconds(travelDelay);
        isTraveling = true;
    }

    private void Update(){
        if(isTraveling)
        {   
            float travelDelta = travelSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, basket.transform.position, travelDelta);

            if(transform.position == basket.transform.position)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy() {
        gameManager.AddPoints(vegetablePoints);
    }
}
