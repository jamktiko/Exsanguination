using System.Collections;
using UnityEngine;

public class EnemyFinisher : MonoBehaviour
{
    private StakeLogic stakeLogic;
    private Animator playerAnimator;
    private InputManager InputManager;
    private Transform playerCamera;
    private AudioSource audioSource;
    private ParticleSystem ps;
    private SkinnedMeshRenderer smr;
    [SerializeField] MeshRenderer stickRenderer;
    [SerializeField] private float finisherTime = 1f;
    private Quaternion startRotation = Quaternion.Euler(-50f, 0, 0);
    private Quaternion endRotation = Quaternion.Euler(0, 0, 0);
    [SerializeField] private float lerpToStartDuration = 1f;
    [SerializeField] private float stayDuration = 3f;
    [SerializeField] private float lerpToEndDuration = 1f;



    private void Awake()
    {
        stakeLogic = GameObject.Find("Stake").GetComponent<StakeLogic>();
        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();
        InputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
        smr = GetComponent<SkinnedMeshRenderer>();

    }

    private void Start()
    {
        stickRenderer.enabled = false;
        smr.enabled = false;
        ps.Stop();

    }



    public void Finish()
    {
        // Trigger the finishing sequence
        smr.enabled = true;
        stickRenderer.enabled = true;
        playerAnimator.SetTrigger("finish");
        StartCoroutine(ShowEnemyFinisher());
        StartCoroutine(RotateCamera());
    }



    private IEnumerator ShowEnemyFinisher()
    {
        playerCamera.transform.localRotation = startRotation;
        InputManager.inputsEnabled = false;
        yield return new WaitForSeconds(finisherTime); 
        ps.Play();
        smr.enabled = false;
        audioSource.PlayOneShot(audioSource.clip);
        yield return new WaitForSeconds(2.3f);
        ps.Stop();
        stickRenderer.enabled = false;
        InputManager.inputsEnabled = true;

    }

    private IEnumerator RotateCamera()
    {
        float lerpTime = 0f;

        while (lerpTime < lerpToEndDuration)
        {
            lerpTime += Time.deltaTime;
            playerCamera.rotation = Quaternion.Slerp(startRotation, endRotation, lerpTime / lerpToEndDuration);
            yield return null;
        }

        playerCamera.rotation = endRotation;
        yield return new WaitForSeconds(stayDuration);

        lerpTime = 0f;

        while (lerpTime < lerpToStartDuration)
        {
            lerpTime += Time.deltaTime;
            playerCamera.rotation = Quaternion.Slerp(endRotation, startRotation, lerpTime / lerpToStartDuration);
            yield return null;
        }

        playerCamera.rotation = startRotation;
    }
}
