using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPick : MonoBehaviour
{

    [SerializeField] float pickingTimer = .5f;

    private CapsuleCollider2D capsuleCollider2D;
    private Animator animator;
    private AudioManager audioManager;

    private Vector2 moveInput;
    private bool isPicking;

    public static event Action OnPick;

    void Awake()
    {   
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        OnPick += PickVegetable;
    }

    private void OnDisable()
    {
        OnPick -= PickVegetable;
    }

    private void Start() {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        CheckForPick();
    }

    //If the player is overlapping a vegetable and pushes down a pick event is triggered
    private void CheckForPick()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("PickVegetable")))
        {
            if (moveInput.y < 0 && !isPicking)
            {
                OnPick?.Invoke();
            }
        }
    }

    private void PickVegetable()
    {
        animator.SetTrigger("isPicking");
        audioManager.PlayPlayerPickSFX();
        isPicking = true;
        StartCoroutine(ResetPickingState());
    }


    IEnumerator ResetPickingState()
    {
        yield return new WaitForSeconds(pickingTimer);
        isPicking = false;
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public bool getIsPicking()
    {
        return isPicking;
    }
}
