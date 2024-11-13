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
    [SerializeField] GameObject smoulderingLineObject;
    [SerializeField] GameObject flamesObject;
    [SerializeField] Vector3 startingPosition;
    [SerializeField] GameObject fireCollider;
    bool flameIsCreated;

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
        StartCoroutine(MoveOverDistance(smoulderingLineObject.transform, smoulderingLineObject.transform.forward, 30f, 0.2f));
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

        if (!flameIsCreated)
        {
            StartCoroutine(WaitBeforeCreatingFlames());
        }
        else
        {
            StartCoroutine(WaitBeforeRemovingFireWall());
        }
    }

    IEnumerator WaitBeforeCreatingFlames()
    {
        yield return new WaitForSeconds(0.5f);

        flameIsCreated = true;
        flamesParticle.Play(true);
        fireCollider.SetActive(true);
        StartCoroutine(MoveOverDistance(flamesObject.transform, flamesObject.transform.forward, 30f, 0.2f));
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
        smoulderingLineObject.transform.localPosition = Vector3.zero;
        flamesObject.transform.localPosition = Vector3.zero;
        fireCollider.transform.localScale = new Vector3(1, 1, 0.1f);
        fireCollider.SetActive(false);
        gameObject.SetActive(false);
    }
}
