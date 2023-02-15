using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button exitButton;

    GameManager gameManager;
    GameObject settingsMenu;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();  
        settingsMenu = FindObjectOfType<SettingsMenu>(true).gameObject;

        startButton.onClick.AddListener(gameManager.LoadNextLevel);
        settingsButton.onClick.AddListener(OpenSettingsMenu);
        exitButton.onClick.AddListener(gameManager.ExitGame);
    }

    void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }    
}
