using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public delegate void OnLevelComplete(int vegetables);
    public static event OnLevelComplete onLevelComplete;

    private bool isTriggered = false;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            //Sends event that level has completed with amount of vegetables 
            //that need to be tallied for points and displayed on the end screen
            onLevelComplete!.Invoke(gameManager.GetVegetableAmount());
        }
    }
}
