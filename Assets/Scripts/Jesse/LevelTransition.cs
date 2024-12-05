using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{

    [SerializeField] Image background;
    [SerializeField] InputHandler inputHandler;
    [SerializeField] float blackFadeTime;
    [SerializeField] float teleportTime;
    [SerializeField] GameObject[] enemiesToKill;
    private MusicManager musicManager;
    private bool allEnemiesInactive;
    [SerializeField] ResetPlayer resetPlayer;
    [Tooltip("Target position where the player prefab will be moved.")]
    public Vector3 teleportTarget;

    private GameObject playerPrefab;
    Rigidbody rb;
   [SerializeField] AudioSource playerFootstepsSource;

    private void Awake()
    {
        background.color = new Color(0, 0, 0, 0);
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        rb = playerPrefab.GetComponent<Rigidbody>();
        
    }



    IEnumerator LevelTransitionTimer(float blackFadeTime)
    {
        // Start playing footsteps in the background
        rb.velocity = Vector3.zero;
        playerFootstepsSource.Stop();
        inputHandler.DisableInput();
        background.enabled = true;
        // Fade the screen to black
        while (background.color.a <= 1)
        {
            background.color = new Color(0, 0, 0, background.color.a + Time.unscaledDeltaTime / blackFadeTime);
            yield return null;
        }

        // Wait for the scene change timer
        yield return new WaitForSecondsRealtime(teleportTime);

        // Stop the footsteps and transition to the next scene
        background.enabled = false;
        resetPlayer.hasTriggered = true;
        playerPrefab.transform.position = teleportTarget;
        inputHandler.EnableInput();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            allEnemiesInactive = true; // Assume all enemies are inactive

            foreach (var enemy in enemiesToKill)
            {
                if (enemy.activeSelf) // Check if any enemy is active
                {
                    allEnemiesInactive = false; // If one is active, set the flag to false
                    break; // Exit the loop early since not all enemies are disabled
                }
            }

            if (allEnemiesInactive)
            {
                StartCoroutine(LevelTransitionTimer(blackFadeTime));
            }
        }
    }
}

