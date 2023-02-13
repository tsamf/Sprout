using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{


    [Header("Mixer")]
    [SerializeField][Range(0, 1)] float masterVolume = 1.0f;
    [SerializeField][Range(0, 1)] float musicVolume = 1.0f;
    [SerializeField][Range(0, 1)] float SFXVolume = 1.0f;
    [SerializeField] AudioSource audioSource;

    [Header("Music")]
    [SerializeField] AudioClip mainMenuTheme;
    [SerializeField][Range(0, 1)] float mainMenuThemeVolume = 1f;

    [SerializeField] AudioClip levelTheme;
    [SerializeField][Range(0, 1)] float levelThemeVolume = 1f;

    [Header("Player")]
    [SerializeField] AudioClip playerJumpSFX;
    [SerializeField][Range(0, 1)] float playerJumpVolume = 1f;

    [SerializeField] AudioClip playerPickSFX;
    [SerializeField][Range(0, 1)] float playerPickVolume = 1f;

    [SerializeField] AudioClip playerThrowSFX;
    [SerializeField][Range(0, 1)] float playerThrowVolume = 1f;

    [SerializeField] AudioClip playerThrowEmptySFX;
    [SerializeField][Range(0, 1)] float playerThrowEmptyVolume = 1f;

    [SerializeField] AudioClip playerHitSFX;
    [SerializeField][Range(0, 1)] float playerHitVolume = 1f;

    [Header("Slime")]
    [SerializeField] AudioClip slimeHitSFX;
    [SerializeField][Range(0, 1)] float slimeHitVolume = 1f;

    [SerializeField] AudioClip slimeDeathSFX;
    [SerializeField][Range(0, 1)] float slimeDeathVolume = 1f;


    private static GameObject instance = null;

    private void OnEnable() {
        GameManager.OnSceneChange += UpdateThemeOnSceneChange;
    }

    private void OnDisable() {
        GameManager.OnSceneChange -= UpdateThemeOnSceneChange;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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

    void Update()
    {
        transform.position = Camera.main.transform.position;
    }

    public void PlayPlayerJumpSFX()
    {
        PlaySFXAtPoint(playerJumpSFX, playerJumpVolume);
    }

    public void PlayPlayerPickSFX()
    {
        PlaySFXAtPoint(playerPickSFX, playerPickVolume);
    }

    public void PlayPlayerHitSFX()
    {
        PlaySFXAtPoint(playerHitSFX, playerHitVolume);
    }

    public void PlayPlayerThrowSFX()
    {
        PlaySFXAtPoint(playerThrowSFX, playerThrowVolume);
    }

    public void PlayplayerThrowEmptySFX()
    {
        PlaySFXAtPoint(playerThrowEmptySFX, playerThrowEmptyVolume);
    }

    public void PlaySlimeHitSFX()
    {
        PlaySFXAtPoint(slimeHitSFX, slimeHitVolume);
    }

    public void PlaySlimeDeathSFX()
    {
        PlaySFXAtPoint(slimeDeathSFX, slimeDeathVolume);
    }

    void PlaySFXAtPoint(AudioClip clip, float clipVolume)
    {
        float playVolume = clipVolume * SFXVolume * masterVolume;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, playVolume);
    }

    void UpdateThemeOnSceneChange()
    {
        string sceneIndex = SceneManager.GetActiveScene().name;

        switch (sceneIndex)
        {
            case "TitleScreen":
            audioSource.clip = mainMenuTheme;
            audioSource.volume = mainMenuThemeVolume * musicVolume * masterVolume; 
                break;
            case "GameOverScreen":
            audioSource.clip = mainMenuTheme;
            audioSource.volume = mainMenuThemeVolume * musicVolume * masterVolume; 
                break;
            default:
            audioSource.clip = levelTheme;
            audioSource.volume = levelThemeVolume * musicVolume * masterVolume; 
                break;
        }
         audioSource.Play();
    }
}
