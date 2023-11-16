using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D cd;
    [SerializeField] private Animator anim;
    private InputActions actions;

    private bool isSprinting;

    public float speed = 5;
    public float sprintMultiplier = 1.2f;

    private Vector2 moveInput;

    private void Awake() {
        actions = new InputActions();
    }

    private void OnEnable() {
        actions.Gameplay.Enable();

        actions.Gameplay.Movement.performed += MoveCharacter;
        actions.Gameplay.Movement.canceled += context => StopMovement();
        actions.Gameplay.Sprint.performed += context => isSprinting = true;
        actions.Gameplay.Sprint.canceled += context => isSprinting = false;

        PauseManager.pauseEvent += TogglePause;
    }

    private void OnDisable() {
        actions.Gameplay.Movement.performed -= MoveCharacter;
        actions.Gameplay.Movement.canceled -= context => StopMovement();
        actions.Gameplay.Sprint.performed -= context => isSprinting = true;
        actions.Gameplay.Sprint.canceled -= context => isSprinting = false;

        PauseManager.pauseEvent -= TogglePause;

        StopMovement();
        isSprinting = false;

        actions.Gameplay.Disable();
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        // reads player input into a vector2
        moveInput = context.ReadValue<Vector2>().normalized;

        // anim.SetBool("Moving", true);
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement()
    {
        float speedToUse = isSprinting ? speed * sprintMultiplier : speed;

        rb.velocity = moveInput * speedToUse;
    }
    private void StopMovement()
    {
        moveInput = Vector2.zero;
        rb.velocity = moveInput;
        // anim.SetBool("Moving", false);
    }

    private void TogglePause(bool pause)
    {
        if (pause)
            actions.Gameplay.Disable();
        else 
            actions.Gameplay.Enable();
    }
}