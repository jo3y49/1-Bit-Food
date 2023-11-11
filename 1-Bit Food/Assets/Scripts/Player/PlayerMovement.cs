using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
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

        anim.SetBool("Moving", true);
    }

    private void StopMovement()
    {
        rb.velocity = Vector3.zero;
        anim.SetBool("Moving", false);
    }
}