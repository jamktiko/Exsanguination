using UnityEngine;

public class PlayerMovementAnimations : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;

    private Vector3 moveDirection;
    private Vector2 horizontalInput; // To store input from RBInputManager

    [SerializeField] private RBInputManager inputManager;

        public void ReceiveInput(Vector2 input)
        {
            // This function is called by RBInputManager to update movement input
            horizontalInput = input;
        }

        void Update()
        {
            // Use the horizontalInput from RBInputManager to calculate move direction
            moveDirection = new Vector3(horizontalInput.x, 0, horizontalInput.y).normalized;

            // Update Animator parameters for the blend tree
            animator.SetFloat("X", moveDirection.x);
            animator.SetFloat("Z", moveDirection.z);
        }
       
}
