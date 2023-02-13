using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button exitButton;

    GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();  
    }

    void Start()
    {
        startButton.onClick.AddListener(gameManager.LoadNextLevel);
        //settingsButton.onClick.AddListener();
        //creditsButton.onClick.AddListener();
        exitButton.onClick.AddListener(gameManager.ExitGame);
    }
}
