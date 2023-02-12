using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField][Range(0,1)] float masterVolume = 1.0f; 
    [SerializeField][Range(0,1)] float musicVolume = 1.0f;
    [SerializeField][Range(0,1)] float SFXVolume = 1.0f;

    [Header("Player")]
    [SerializeField] AudioClip playerJumpSFX;
    [SerializeField][Range(0,1)] float playerJumpVolume = 1f;

    [SerializeField] AudioClip playerPickSFX;
    [SerializeField][Range(0,1)] float playerPickVolume = 1f;

    [SerializeField] AudioClip playerThrowSFX;
    [SerializeField][Range(0,1)] float playerThrowVolume = 1f;

    [SerializeField]AudioClip playerThrowEmptySFX;
    [SerializeField][Range(0,1)] float playerThrowEmptyVolume = 1f; 

    [SerializeField] AudioClip playerHitSFX;
    [SerializeField][Range(0,1)] float playerHitVolume = 1f;


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
        PlaySFXAtPoint(playerPickSFX,playerPickVolume);
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
        PlaySFXAtPoint(playerThrowEmptySFX,playerThrowEmptyVolume); 
    }

    void PlaySFXAtPoint(AudioClip clip,float clipVolume)
    {
        float playVolume = clipVolume * SFXVolume * masterVolume;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, playVolume);
    }
}
