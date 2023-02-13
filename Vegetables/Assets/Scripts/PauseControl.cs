using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseControl : MonoBehaviour
{
    
    private GameObject pauseMenu;
    
    private float previousTimeScale = 1.0f;
    public static bool isPaused = false;

   
    private void Awake()
    {
        pauseMenu = FindObjectOfType<PauseMenu>(true).gameObject;
    }

    private void OnPause(InputValue value)
    {
        if (value.isPressed)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
            isPaused = true;
            pauseMenu.SetActive(true); 
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = previousTimeScale;
            AudioListener.pause = false;
            isPaused = false;
            pauseMenu.SetActive(false); 
        }
    }
}
