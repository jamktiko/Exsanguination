using UnityEngine;

public class PlayerMovementAnimations : MonoBehaviour
{
    [SerializeField] Animator animator;  // Reference to your Animator component
    InputManager inputManager;  // Reference to your RBInputManager script
    PlayerMovement playerMovement;

    // Parameters for Blend Tree control
    private static readonly int XParam = Animator.StringToHash("X");
    private static readonly int ZParam = Animator.StringToHash("Z");

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (!playerMovement.isSliding)
        {
            // Get the horizontal input from the RBInputManager
            Vector2 horizontalInput = inputManager.GetHorizontalInput();  // Create a getter for this in RBInputManager

            // Set the animator parameters based on movement input
            animator.SetFloat(XParam, horizontalInput.x);
            animator.SetFloat(ZParam, horizontalInput.y);
        }
        
    }

}
