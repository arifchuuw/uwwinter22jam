using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;

    [SerializeField]
    public GameObject playerPrefab;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        var player1 = PlayerInput.Instantiate(prefab: playerPrefab, playerIndex: 0, controlScheme: "Keyboard1", pairWithDevice: Keyboard.current, splitScreenIndex: 0);
        var player2 = PlayerInput.Instantiate(prefab: playerPrefab, playerIndex: 1, controlScheme: "Keyboard2", pairWithDevice: Keyboard.current, splitScreenIndex: 1);
    }

    // Callback function that is called when we trigger the given input
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        // When player is in contact with the ground
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Rotates the player in the direction they are facing
        //if (move != Vector3.zero)
        //{
        //    gameObject.transform.forward = move;
        //}

        // Changes the height position of the player..
        if (jumped && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        // Based on gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}