using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverMenu : MonoBehaviour
{

    [Header("Text")]
    [SerializeField] TextMeshProUGUI enemiesKilledText;
    [SerializeField] TextMeshProUGUI vegetablesCollectedText;
    [SerializeField] TextMeshProUGUI totalPointsText;

    [Header("Buttons")]
    [SerializeField] Button restartButton;
    [SerializeField] Button mainMenu;
    [SerializeField] Button exitButton;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        enemiesKilledText.text = "Enemies Killed: " + gameManager.GetEnemiesKilled().ToString();
        vegetablesCollectedText.text = "Vegetables Collected: " + gameManager.GetVegetablesTrunedIn();
        totalPointsText.text = "Total Points: " + gameManager.GetPlayerScore().ToString();

        restartButton.onClick.AddListener(gameManager.LoadFirstLevel);
        mainMenu.onClick.AddListener(gameManager.LoadMainMenu);
        exitButton.onClick.AddListener(gameManager.ExitGame);
    }
}
