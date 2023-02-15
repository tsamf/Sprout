using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] int playerVegetableCount = 0;
    [SerializeField] int playerScore = 0;
    [SerializeField] float countSpeed = .01f;
    [SerializeField] float levelChangeEventDelay = .1f;

    public static event Action OnPointUpdate;
    public static event Action OnVegetableCountUpdate;
    public static event Action OnSceneChange;

    private static GameObject instance = null;

    private int vegetablesTurnedIn = 0;
    private int enemiesKilled = 0;

    private void Awake()
    {
        CreateSingleton();
    }

    private void OnEnable()
    {
        Basket.onLevelComplete += turnInVegetables;
    }

    private void OnDisable()
    {
        Basket.onLevelComplete -= turnInVegetables;
    }

    private void CreateSingleton()
    {
        if (instance == null)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void RemoveVegetables(int amount)
    {
        StartCoroutine(DecrementVegetables(amount));
    }

    IEnumerator DecrementVegetables(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //The player cant have a negative amount of vegetables
            if (playerVegetableCount > 0)
            {
                playerVegetableCount--;
                OnVegetableCountUpdate!.Invoke();
            }
            yield return new WaitForSeconds(countSpeed);
        }
    }

    public void AddVegetables(int amount)
    {
        StartCoroutine(IncermentVegetables(amount));
    }

    IEnumerator IncermentVegetables(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            playerVegetableCount++;
            OnVegetableCountUpdate!.Invoke();
            yield return new WaitForSeconds(countSpeed);
        }
    }

    public void AddPoints(int amount)
    {
        StartCoroutine(IncrementPoints(amount));
    }

    public void IncrementEnemiesKilled()
    {
        enemiesKilled++;
    }

    IEnumerator IncrementPoints(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            playerScore++;
            OnPointUpdate!.Invoke();
            yield return new WaitForSeconds(countSpeed);
        }
    }

    public void LoadNextLevel()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(FireSceneChangeEventAfterDelay());
    }

    public void LoadFirstLevel()
    {
        ResetVariables();
        StopAllCoroutines();
        SceneManager.LoadScene(1);
        StartCoroutine(FireSceneChangeEventAfterDelay());
    }

    public void LoadMainMenu()
    {
        ResetVariables();
        StopAllCoroutines();
        SceneManager.LoadScene(0);
        StartCoroutine(FireSceneChangeEventAfterDelay());
    }

    public void ResetVariables()
    {
        vegetablesTurnedIn = 0;
        enemiesKilled = 0;
        playerScore = 0;
        playerVegetableCount = 0;
    }

    //Allows other persistent scripts to make changes based on level changing event 
    IEnumerator FireSceneChangeEventAfterDelay()
    {
        yield return new WaitForSeconds(levelChangeEventDelay);
        OnSceneChange!.Invoke();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Exit Game was pushed.");
#endif

        Application.Quit();
    }

    private void turnInVegetables(int vegetables)
    {
        vegetablesTurnedIn += vegetables;
    }

    public int GetVegetableAmount()
    {
        return playerVegetableCount;
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

     public int GetVegetablesTrunedIn()
    {
        return vegetablesTurnedIn;
    }

    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }
}
