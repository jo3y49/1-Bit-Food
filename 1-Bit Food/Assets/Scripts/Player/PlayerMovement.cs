using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D cd;
    [SerializeField] private Animator anim;
    private InputActions actions;

    public float walkSpeed = 5;
    public float sprintSpeed = 8;

    private bool isSprinting = false;

    private void Awake() {
        actions = new InputActions();
    }

    private void OnEnable() {
        actions.Gameplay.Enable();

        actions.Gameplay.Movement.performed += MoveCharacter;
        actions.Gameplay.Movement.canceled += context => StopMovement();
        actions.Gameplay.Sprint.performed += context => isSprinting = true;
        actions.Gameplay.Sprint.canceled  += context => isSprinting = false;
    }

    private void OnDisable() {
        actions.Gameplay.Movement.performed -= MoveCharacter;
        actions.Gameplay.Movement.canceled -= context => StopMovement();
        actions.Gameplay.Sprint.performed -= context => isSprinting = true;
        actions.Gameplay.Sprint.canceled  -= context => isSprinting = false;

        StopMovement();

        actions.Gameplay.Disable();
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        // reads player input into a vector2
        Vector2 moveInput = context.ReadValue<Vector2>().normalized;

        // checks if player is sprinting
        float speedToUse = isSprinting ? sprintSpeed : walkSpeed;

        rb.velocity = moveInput * speedToUse;

        CheckForCollision();

        anim.SetBool("Moving", true);
    }

    private void FixedUpdate() {
        CheckForCollision();
    }
    private void StopMovement()
    {
        rb.velocity = Vector3.zero;
        anim.SetBool("Moving", false);
    }

    private void CheckForCollision()
    {
        float left = cd.bounds.min.x;
        float right = cd.bounds.max.x;
        float top = cd.bounds.max.y;
        float bottom = cd.bounds.min.y;
        float middleX = cd.bounds.center.x;
        float middleY = cd.bounds.center.y;

        Vector2 topLeft = new(left, top);
        Vector2 middleLeft = new(left, middleY);
        Vector2 bottomLeft = cd.bounds.min;

        Vector2 topRight = cd.bounds.max;
        Vector2 middleRight = new(right, middleY);
        Vector2 bottomRight = new(right, bottom);

        Vector2 topMiddle = new(middleX, top);
        Vector2 bottomMiddle = new(middleX, bottom);

        float rayLength = .5f;
        float collisionOffset = .1f;

        Vector2 newVelocity = rb.velocity;

        List<List<RaycastHit2D>> rays = new()
        {
            // Left rays
            new List<RaycastHit2D>
            {
                Physics2D.Raycast(topLeft, Vector2.left, rayLength),
                Physics2D.Raycast(middleLeft, Vector2.left, rayLength),
                Physics2D.Raycast(bottomLeft, Vector2.left, rayLength)
            },

            // Right rays
            new List<RaycastHit2D>
            {
                Physics2D.Raycast(topRight, Vector2.right, rayLength),
                Physics2D.Raycast(middleRight, Vector2.right, rayLength),
                Physics2D.Raycast(bottomRight, Vector2.right, rayLength)
            },

            // Top rays
            new List<RaycastHit2D>
            {
                Physics2D.Raycast(topLeft, Vector2.up, rayLength),
                Physics2D.Raycast(topMiddle, Vector2.up, rayLength),
                Physics2D.Raycast(topRight, Vector2.up, rayLength)
            },

            // Bottom rays
            new List<RaycastHit2D>
            {
                Physics2D.Raycast(bottomLeft, Vector2.down, rayLength),
                Physics2D.Raycast(bottomMiddle, Vector2.down, rayLength),
                Physics2D.Raycast(bottomRight, Vector2.down, rayLength)
            }
        };

        foreach (List<RaycastHit2D> rayList in rays)
        {
            foreach (RaycastHit2D ray in rayList)
            {
                if (ray.collider != null)
                {
                    // Move the player slightly away from the point of collision
                    Vector2 offsetDirection = ray.normal * collisionOffset;
                    transform.position = new Vector2(transform.position.x + offsetDirection.x, transform.position.y + offsetDirection.y);

                    break;
                }
            }
        }
    }
}