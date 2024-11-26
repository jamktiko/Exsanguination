using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    [Tooltip("Target position where the player prefab will be moved.")]
    public Vector3 teleportTarget;

    [Tooltip("Tag that identifies the player.")]
    public string playerTag = "Player";

    [SerializeField] ResetPlayer resetPlayer;
   
    private GameObject playerPrefab;

    private void Awake()
    {
        playerPrefab = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the correct tag
        if (other.CompareTag(playerTag) && playerPrefab != null)
        {
            // Teleport the player prefab to the specified position
            resetPlayer.hasTriggered = true;
            playerPrefab.transform.position = teleportTarget;
        }
    }

   
}
