using System.Collections;
using UnityEngine;

public class EnemyFinisher : MonoBehaviour
{
    private StakeLogic stakeLogic;
    private Animator playerAnimator;
    private InputManager InputManager;
    private Transform playerCamera;
   [SerializeField] private GameObject finishedEnemyModel;
    [SerializeField] private float finisherTime = 1f;
    private Quaternion startRotation = Quaternion.Euler(-50f, 0, 0);
    private Quaternion endRotation = Quaternion.Euler(0, 0, 0);
    [SerializeField] private float lerpToStartDuration = 0.5f;
    [SerializeField] private float stayDuration = 0.8f;
    [SerializeField] private float lerpToEndDuration = 0.2f;



    private void Awake()
    {
        finishedEnemyModel = GameObject.Find(gameObject.name + "Finish");
        stakeLogic = GameObject.Find("Stake").GetComponent<StakeLogic>();
        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();
        InputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    private void Start()
    {
        finishedEnemyModel.SetActive(false);
    }

    public void Finish()
    {
        // Trigger the finishing sequence
        stakeLogic.UnstickFromEnemy();
        stakeLogic.ReturnToPlayer();
        playerAnimator.SetTrigger("finish");
        StartCoroutine(ShowEnemyFinisher());
        StartCoroutine(RotateCamera());
    }



    private IEnumerator ShowEnemyFinisher()
    {
        playerCamera.transform.localRotation = startRotation;
        InputManager.inputsEnabled = false;
        finishedEnemyModel.SetActive(true);
        yield return new WaitForSeconds(finisherTime);

        ParticleSystem ps = finishedEnemyModel.GetComponent<ParticleSystem>();
        ps.Play();

        SkinnedMeshRenderer smr = finishedEnemyModel.GetComponent<SkinnedMeshRenderer>();
        smr.enabled = false;

        yield return new WaitForSeconds(0.5f);

        ps.Stop();
        smr.enabled = true;
        finishedEnemyModel.SetActive(false);
        InputManager.inputsEnabled = true;

        // Call Die method on the EnemyHealthScript if needed here
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
