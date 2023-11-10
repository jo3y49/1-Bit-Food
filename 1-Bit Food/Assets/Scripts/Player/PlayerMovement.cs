using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    private InputActions actions;

    public float walkSpeed = 5;

    private void Awake() {
        actions = new InputActions();
    }

    private void OnEnable() {
        actions.Gameplay.Enable();

        actions.Gameplay.Movement.performed += MoveCharacter;
        actions.Gameplay.Movement.canceled += StopCharacter;
    }

    private void OnDisable() {
        actions.Gameplay.Movement.performed -= MoveCharacter;
        actions.Gameplay.Movement.canceled -= StopCharacter;

        StopMovement();

        actions.Gameplay.Disable();
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        anim.SetBool("Moving", true);

        Vector3 moveDirection = new Vector3(moveInput.x, moveInput.y, 0).normalized;

        float speedToUse = walkSpeed;

        rb.velocity = moveDirection * speedToUse;
    }

    private void StopMovement()
    {
        rb.velocity = Vector3.zero;
        anim.SetBool("Moving", false);
    }

    private void StopCharacter(InputAction.CallbackContext context)
    {
        StopMovement();
    }
}