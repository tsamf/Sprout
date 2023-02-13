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
    GameObject settingsMenu;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();  
        settingsMenu = FindObjectOfType<SettingsMenu>(true).gameObject;
    }

    void Start()
    {
        startButton.onClick.AddListener(gameManager.LoadNextLevel);
        settingsButton.onClick.AddListener(OpenSettingsMenu);
        //creditsButton.onClick.AddListener();
        exitButton.onClick.AddListener(gameManager.ExitGame);
    }

    void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }    
}
