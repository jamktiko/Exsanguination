using System.Collections;
using UnityEngine;

public class EnemyFinisher : MonoBehaviour
{
    private Animator playerAnimator;
    private InputHandler InputManager;
    private Transform playerCamera;
    private MouseLook mLook;
    [SerializeField] private AudioSource finisherGhoulAudioSource;
    [SerializeField] private GameObject finisherGhoulParticleSystem;
    [SerializeField] private SkinnedMeshRenderer finisherGhoulRenderer;
    [SerializeField] MeshRenderer finisherstickRenderer;
    private AudioManager audioManager;
    public float duration; // Time to complete the rotation

    private Quaternion targetRotation; // Desired target rotation
    private Quaternion startRotation;  // Starting rotation
    private float elapsedTime = 0f;    // Time passed since the start of the rotation
    private bool isRotating = false;   // Flag to track if a rotation is happening
   [SerializeField] private string enemyType;
    private SkinnedMeshRenderer enemyMesh;
    private AudioSource finisherAudio;
    private AudioSource enemyDeathAudio;
    private GameObject enemyParticleSystem;
    private GameObject tmpParticle;
    [SerializeField] private GameObject particleInstantiateSpot;
    private Rigidbody rb;
    public bool isFinishing;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        InputManager = GetComponentInParent<InputHandler>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        mLook = GetComponentInParent<MouseLook>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        rb = GetComponentInParent<Rigidbody>();

    }

    private void Start()
    {
        finisherstickRenderer.enabled = false;
        finisherGhoulRenderer.enabled = false;
        //finisherGhoulParticleSystem.Stop();
    }

    void Update()
    {
        if (isRotating)
        {
            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the fraction of the duration that has passed
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Smoothly rotate the camera using Slerp (spherical interpolation)
            playerCamera.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Stop rotating once the duration has passed
            if (t >= 1.0f)
            {
                isRotating = false;
            }
        }
    }

    public void SetEnemyType(string Enemytype)
    {
        enemyType = Enemytype;

        SetEnemySettings();
    }

    private void SetEnemySettings()
    {
        if(enemyType == "EnemyGhoul")
        {
            enemyParticleSystem = finisherGhoulParticleSystem;
            enemyMesh = finisherGhoulRenderer;
            enemyDeathAudio = finisherGhoulAudioSource;

        }
    }

    public void Finish()
    {
        audioManager.PlayStakeFinisherAudioClip();
        enemyMesh.enabled = true;          
        finisherstickRenderer.enabled = true;
        playerAnimator.SetTrigger("finish");
        InputManager.DisableInput();
        mLook.enabled = false;
        isFinishing = true;
    }

    public void EnemyExplode()
    {

        //enemyParticleSystem.Play();
        tmpParticle = Instantiate(enemyParticleSystem, particleInstantiateSpot.transform);
        enemyMesh.enabled = false;
        enemyDeathAudio.PlayOneShot(finisherGhoulAudioSource.clip);

    }

    public void FinishFinisherAnimation()
    {
        //enemyParticleSystem.Stop();
        isFinishing = false;
        finisherstickRenderer.enabled = false;
        InputManager.EnableInput();
        mLook.enabled = true;
        Destroy(tmpParticle);
        tmpParticle = null;
    }

    public void RotateToTarget()
    {
        if (!isRotating)
        {
            startRotation =  Quaternion.Euler(playerCamera.rotation.x, 0,0);               // Record the current rotation
            targetRotation = Quaternion.Euler(-50, 0, 0);     // Set target rotation
            elapsedTime = 0f;                                 // Reset elapsed time
            isRotating = true;                                // Set rotation flag
        }
    }

    // Method to rotate the camera back to the initial rotation
    public void RotateBack()
    {
        if (!isRotating)
        {

            startRotation = Quaternion.Euler(-50, 0, 0);               // Record the current rotation
            targetRotation = Quaternion.Euler(0, 0, 0);       // Set target rotation back to (0,0,0)
            elapsedTime = 0f;                                 // Reset elapsed time
            isRotating = true;                                // Set rotation flag
        }
    }


}
