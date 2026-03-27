using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Player))]
public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    Player player;

    void OnEnable()
    {
        playerInput.actions["Move"].performed += OnPlayerMove;
        playerInput.actions["Move"].canceled += OnPlayerMove;

        playerInput.actions["Attack"].performed += PlayerAttack;

        playerInput.actions["Interact"].performed += OnInteract;

        playerInput.actions["Jump"].performed += OnJump;
        playerInput.actions["Jump"].canceled += OnJump;

        playerInput.actions["Sprint"].performed += OnSprint;
    }

    void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnPlayerMove;
        playerInput.actions["Move"].canceled -= OnPlayerMove;

        playerInput.actions["Attack"].performed -= PlayerAttack;

        playerInput.actions["Interact"].performed -= OnInteract;

        playerInput.actions["Jump"].performed -= OnJump;
        playerInput.actions["Jump"].canceled -= OnJump;

        playerInput.actions["Sprint"].performed -= OnSprint;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        player = GetComponent<Player>();
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            player.StartJump();
            return;
        }
        else if (context.canceled)
            player.EndJump();
    }

    void PlayerAttack(InputAction.CallbackContext context)
    {
        
    }

    void OnPlayerMove(InputAction.CallbackContext context)
    {
        player.SetMoveDirection(context.ReadValue<Vector2>().x);
    }
}
