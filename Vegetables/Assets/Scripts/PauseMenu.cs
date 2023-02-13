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

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        pauseControl = FindObjectOfType<PauseControl>();    
    }

    void Start()
    {
        continueButton.onClick.AddListener(pauseControl.TogglePause);
        //settingsButton.onClick.AddListener();
        goToMainButton.onClick.AddListener(GoToMainMenu);
        exitButton.onClick.AddListener(gameManager.ExitGame);
    }

    void GoToMainMenu()
    {
        pauseControl.TogglePause();
        gameManager.LoadMainMenu();
    }
}
