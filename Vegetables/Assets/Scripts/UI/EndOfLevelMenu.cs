using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndOfLevelMenu : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] TextMeshProUGUI vegetableCountText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Button nextLevelButton;

    private GameObject playerUI;
    private GameManager gameManager;
    private float timeScale;

    public static bool isLevelOver = false;

    private void Awake()
    {
        playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        PlayerDrop.onAllVegetablesDropped += LoadMenu;
        timeScale = Time.timeScale;
    }

    private void OnDisable()
    {
        PlayerDrop.onAllVegetablesDropped -= LoadMenu;
    }

    private void Start()
    {
        nextLevelButton.onClick.AddListener(GoToNextLevel);
    }

    private void LoadMenu(int amount)
    {
        isLevelOver = true;
        playerUI.SetActive(false);
        menu.SetActive(true);
        Time.timeScale = 0;
        vegetableCountText.text = "Vegetables turned in:" + amount.ToString();
        scoreText.text = "Current score:" + gameManager.GetPlayerScore().ToString();
    }

    private void GoToNextLevel()
    {
        Time.timeScale = timeScale;
        gameManager.LoadNextLevel();
    }
}
