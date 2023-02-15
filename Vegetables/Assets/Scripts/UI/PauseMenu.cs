using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] Button continueButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button goToMainButton;
    [SerializeField] Button exitButton;

    GameManager gameManager;
    PauseControl pauseControl;
    GameObject settingsMenu;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        pauseControl = FindObjectOfType<PauseControl>();  
        settingsMenu = FindObjectOfType<SettingsMenu>(true).gameObject; 

        continueButton.onClick.AddListener(pauseControl.TogglePause);
        settingsButton.onClick.AddListener(OpenSettings);
        goToMainButton.onClick.AddListener(GoToMainMenu);
        exitButton.onClick.AddListener(gameManager.ExitGame);
    }

    void GoToMainMenu()
    {
        pauseControl.TogglePause();
        gameManager.LoadMainMenu();
    }

    void OpenSettings()
    {
        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
