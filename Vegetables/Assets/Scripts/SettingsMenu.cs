using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider master;
    [SerializeField] Slider music;
    [SerializeField] Slider SFX;
    [SerializeField] Button save;

    private GameObject pauseMenu;
    private GameObject startMenu;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        save.onClick.AddListener(SaveSettings);
    }

    private void OnEnable()
    {
        master.value = audioManager.MasterVolume;
        music.value = audioManager.MusicVolume;
        SFX.value = audioManager.SFXVolume;
        
        try{pauseMenu = FindObjectOfType<PauseMenu>(true).gameObject;}catch{}
        try{startMenu = FindObjectOfType<StartMenu>(true).gameObject;}catch{}
    }

    void SaveSettings()
    {
        audioManager.MasterVolume = master.value;
        audioManager.MusicVolume = music.value;
        audioManager.SFXVolume = SFX.value;

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }

        if (startMenu != null)
        {
            startMenu.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
