using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirewallBehaviour : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1, 1, 1);
    [SerializeField] float fireWallLength;
    [SerializeField] float firewallHorizontalGrowthSpeed;
    [SerializeField] float firewallVerticalShrinkSpeed;
    [SerializeField] GameObject smoulderingLine;
    [SerializeField] GameObject flames;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] GameObject fireCollider;
    bool firewallIsCreated;

    [SerializeField] ParticleSystem smoulderingLineParticle;
    [SerializeField] ParticleSystem flamesParticle;

    private void Awake()
    {
        gameObject.SetActive(false);
        fireCollider.SetActive(false);
        startingPosition = transform.position;
    }

    private void OnEnable()
    {
        smoulderingLineParticle.Play(true);
        StartCoroutine(MoveOverDistance(smoulderingLine.transform, smoulderingLine.transform.forward, 10f, 0.5f));
    }

    private IEnumerator MoveOverDistance(Transform target, Vector3 direction, float distance, float duration)
    {
        Vector3 startPosition = target.position;
        Vector3 endPosition = startPosition + direction.normalized * distance;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.position = endPosition; // Ensure the object reaches the exact endpoint

        if (!firewallIsCreated)
        {
            StartCoroutine(WaitBeforeCreatingFirewall());
        }
        else
        {
            StartCoroutine(WaitBeforeRemovingFireWall());
        }
    }

    IEnumerator WaitBeforeCreatingFirewall()
    {
        yield return new WaitForSeconds(1.5f);

        firewallIsCreated = true;
        flamesParticle.Play(true);
        fireCollider.SetActive(true);
        StartCoroutine(MoveOverDistance(flames.transform, flames.transform.forward, 10f, 0.5f));
        StartCoroutine(ScaleOverTime(targetScale, 0.5f));
    }

    private IEnumerator ScaleOverTime(Vector3 target, float duration)
    {
        Vector3 initialScale = fireCollider.transform.localScale;
        float time = 0;

        while (time < duration)
        {
            fireCollider.transform.localScale = Vector3.Lerp(initialScale, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = target; // Ensure final scale is exactly the target scale
    }

    IEnumerator WaitBeforeRemovingFireWall()
    {
        yield return new WaitForSeconds(5);

        RemoveFirewall();
    }

    private void RemoveFirewall()
    {
        smoulderingLineParticle.Stop();
        flamesParticle.Stop();
        transform.position = startingPosition;
        smoulderingLine.transform.localPosition = Vector3.zero;
        flames.transform.localPosition = Vector3.zero;
        fireCollider.transform.localScale = new Vector3(1, 1, 0.1f);
        fireCollider.SetActive(false);
    }
}
