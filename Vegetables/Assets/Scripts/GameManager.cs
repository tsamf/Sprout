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
    [SerializeField] int pointsPerVegetable = 5;


    public static event Action OnPointUpdate;
    public static event Action OnVegetableCountUpdate;
    public static event Action OnSceneChange;

    private static GameObject instance = null;

    private void Awake()
    {
        CreateSingleton();
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

    public int GetVegetableAmount()
    {
        return playerVegetableCount;
    }

    public int GetPlayerScore()
    {
        return playerScore;
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
        StopAllCoroutines();
        SceneManager.LoadScene(1);
        StartCoroutine(FireSceneChangeEventAfterDelay());
    }

    public void LoadMainMenu()
    {
        playerScore = 0;
        playerVegetableCount = 0;
        StopAllCoroutines();
        SceneManager.LoadScene(0);
        StartCoroutine(FireSceneChangeEventAfterDelay());
    }

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
}
