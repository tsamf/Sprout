using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionTrigger : MonoBehaviour
{
    [SerializeField] GameObject triggerObject;
    [SerializeField] GameObject confirmImage;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == triggerObject.tag)
        {
            confirmImage.SetActive(true);
        }
    }
}
