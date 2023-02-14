using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] Button mainMenu;
    [SerializeField] Button exitButton;

    GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();  
    }

    void Start()
    {
        restartButton.onClick.AddListener(gameManager.LoadFirstLevel);
        mainMenu.onClick.AddListener(gameManager.LoadMainMenu);
        exitButton.onClick.AddListener(gameManager.ExitGame);
    }
}
