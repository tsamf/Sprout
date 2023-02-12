using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] int playerVegetableCount = 0;
    [SerializeField] TextMeshProUGUI vegetableText; 
    [SerializeField] float countSpeed= .01f;
    
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

    public void RemoveVegetables(int amount)
    {
        StartCoroutine(DecrementVegetables(amount));
    }

    IEnumerator DecrementVegetables(int count)
    {
        for(int i =0; i < count; i++)
        {
            //The player cant have a negative amount of vegetables
            if(playerVegetableCount > 0)
            {
                playerVegetableCount--;
                vegetableText.text = "Vegetables:" + playerVegetableCount.ToString();
            }
            yield return new WaitForSeconds(countSpeed);
        }
    }

    public void AddVegetables(int amount)
    {
        StartCoroutine(incermentVegetables(amount));
    }

    IEnumerator incermentVegetables(int count)
    {
        for(int i =0; i < count; i++)
        {
            playerVegetableCount++;
            vegetableText.text = "Vegetables:" + playerVegetableCount.ToString();
            yield return new WaitForSeconds(countSpeed);
        }
    }
    
    public void LoadNextLevel()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            Debug.Log("Exit Game was pushed.");
        #endif

        Application.Quit();
    }
}
