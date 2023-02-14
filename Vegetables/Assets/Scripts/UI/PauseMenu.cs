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

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        pauseControl = FindObjectOfType<PauseControl>();    
    }

    private void OnEnable(){
        settingsMenu = FindObjectOfType<SettingsMenu>(true).gameObject; 
    }

    void Start()
    {
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
