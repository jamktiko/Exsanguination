using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [SerializeField] Rigidbody[] ragdollRigidbodies;  // Array of enemy's body part rigidbodies
    [SerializeField] Collider[] ragdollColliders;     // Array of enemy's body part colliders
    [SerializeField] Animator enemyAnimator;          // Enemy's Animator
    [SerializeField] Collider mainCollider;           // Main collider for the enemy (for normal movement)

    void Start()
    {
        // Initialize and disable the ragdoll at the start
        ToggleRagdoll(false);
    }

    // Method to toggle ragdoll state (true to enable, false to disable)
    private void ToggleRagdoll(bool isRagdoll)
    {
        // Disable the Animator and main collider when ragdolling
        enemyAnimator.enabled = !isRagdoll;
        mainCollider.enabled = !isRagdoll;

        // Loop through all the ragdoll body parts and toggle colliders/rigidbodies
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !isRagdoll;  // Enable physics on ragdoll when true
        }

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = isRagdoll;  // Enable colliders for ragdoll parts
        }
    }

    // Call this method when the enemy should ragdoll, e.g., during a finisher
    public void ActivateRagdoll()
    {
        ToggleRagdoll(true);
    }

    // Example usage: Deactivate ragdoll (back to animated state)
    public void DeactivateRagdoll()
    {
        ToggleRagdoll(false);
    }
}
