using System.Collections;
using UnityEngine;

public class EnemyFinisher : MonoBehaviour
{
    [SerializeField] private StakeLogic stakeLogic;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private InputManager InputManager;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private GameObject ghoulFinishedEnemy;
    [SerializeField] private float finisherTime = 1f;
    [SerializeField] private Quaternion startRotation = Quaternion.Euler(-50f, 0, 0);
    [SerializeField] private Quaternion endRotation = Quaternion.Euler(0, 0, 0);
    [SerializeField] private float lerpToEndDuration = 0.2f;
    [SerializeField] private float stayDuration = 0.8f;
    [SerializeField] private float lerpToStartDuration = 0.5f;

    private void Start()
    {
        ghoulFinishedEnemy.SetActive(false);
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
        InputManager.ControlsEnabled(false);
        ghoulFinishedEnemy.SetActive(true);
        yield return new WaitForSeconds(finisherTime);

        ParticleSystem ps = ghoulFinishedEnemy.GetComponent<ParticleSystem>();
        ps.Play();

        SkinnedMeshRenderer smr = ghoulFinishedEnemy.GetComponent<SkinnedMeshRenderer>();
        smr.enabled = false;

        yield return new WaitForSeconds(0.5f);

        ps.Stop();
        smr.enabled = true;
        ghoulFinishedEnemy.SetActive(false);
        InputManager.ControlsEnabled(true);

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
