using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPickTrigger : MonoBehaviour
{
    [SerializeField] GameObject confirmImage;
    private AudioManager audioManager;

    private bool isTriggered = false;

    private void OnEnable()
    {
        PlayerPick.OnPick += pickCheck;
    }

    private void OnDisable()
    {
        PlayerPick.OnPick -= pickCheck;
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void pickCheck()
    {
        if (!isTriggered)
        {
            confirmImage.SetActive(true);
        }
    }
}
