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

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            onLevelComplete!.Invoke(gameManager.GetVegetableAmount());
        }
    }
}
