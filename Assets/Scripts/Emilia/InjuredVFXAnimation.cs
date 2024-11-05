using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InjuredVFXAnimation : MonoBehaviour
{
    [SerializeField] Image InjuredVFX0; // Least blood
    [SerializeField] Image InjuredVFX1; // Moderate blood
    [SerializeField] Image InjuredVFX2; // Most blood
    [SerializeField] float fadeDuration = 2.0f; // Duration for fade effect
    [SerializeField] float resetDelay = 1.0f; // Delay before restarting animation

    private bool coroutineRunning = false;
    private float maxAlpha; // Maximum alpha based on health

    private void Awake()
    {
        // Ensure all VFX images start inactive
        InjuredVFX0.gameObject.SetActive(false);
        InjuredVFX1.gameObject.SetActive(false);
        InjuredVFX2.gameObject.SetActive(false);
    }

    public void StartInjuredVFXAnimation(int health)
    {
        // Set max alpha based on health value
        if (health <= 50)
        {
            maxAlpha = 1 - (health / 50f);
        }
        else
        {
            maxAlpha = 0;
        }

        if (maxAlpha > 0)
        {
            StopInjuredVFXAnimation(); //Avoid effect stacking
            StartCoroutine(FadeInAndOutVFX());
        }
        else
        {
            StopInjuredVFXAnimation();
        }
    }

    IEnumerator FadeInAndOutVFX()
    {
        coroutineRunning = true;
        // Enable all VFX images for animation
        InjuredVFX0.gameObject.SetActive(true);
        InjuredVFX1.gameObject.SetActive(true);
        InjuredVFX2.gameObject.SetActive(true);

        float elapsedTime = 0f;

        // Set initial alpha values for the images based on maxAlpha
        InjuredVFX0.color = new Color(InjuredVFX0.color.r, InjuredVFX0.color.g, InjuredVFX0.color.b, maxAlpha);
        InjuredVFX1.color = new Color(InjuredVFX1.color.r, InjuredVFX1.color.g, InjuredVFX1.color.b, 0);
        InjuredVFX2.color = new Color(InjuredVFX2.color.r, InjuredVFX2.color.g, InjuredVFX2.color.b, 0);

        // Phase 1: Fade InjuredVFX0 out and InjuredVFX1 in
        while (elapsedTime < fadeDuration)
        {
            float alpha = elapsedTime / fadeDuration * maxAlpha;
            InjuredVFX0.color = new Color(InjuredVFX0.color.r, InjuredVFX0.color.g, InjuredVFX0.color.b, maxAlpha - alpha);
            InjuredVFX1.color = new Color(InjuredVFX1.color.r, InjuredVFX1.color.g, InjuredVFX1.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset elapsed time
        elapsedTime = 0f;

        // Phase 2: Fade InjuredVFX1 out and InjuredVFX2 in
        while (elapsedTime < fadeDuration)
        {
            float alpha = elapsedTime / fadeDuration * maxAlpha;
            InjuredVFX1.color = new Color(InjuredVFX1.color.r, InjuredVFX1.color.g, InjuredVFX1.color.b, maxAlpha - alpha);
            InjuredVFX2.color = new Color(InjuredVFX2.color.r, InjuredVFX2.color.g, InjuredVFX2.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Brief delay with InjuredVFX2 fully visible
        yield return new WaitForSeconds(resetDelay);

        // Start the fade-back sequence: InjuredVFX2 → InjuredVFX1 → InjuredVFX0
        elapsedTime = 0f;

        // Phase 3: Fade InjuredVFX2 out and InjuredVFX1 in
        while (elapsedTime < fadeDuration)
        {
            float alpha = elapsedTime / fadeDuration * maxAlpha;
            InjuredVFX2.color = new Color(InjuredVFX2.color.r, InjuredVFX2.color.g, InjuredVFX2.color.b, maxAlpha - alpha);
            InjuredVFX1.color = new Color(InjuredVFX1.color.r, InjuredVFX1.color.g, InjuredVFX1.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset elapsed time
        elapsedTime = 0f;

        // Phase 4: Fade InjuredVFX1 out and InjuredVFX0 in
        while (elapsedTime < fadeDuration)
        {
            float alpha = elapsedTime / fadeDuration * maxAlpha;
            InjuredVFX1.color = new Color(InjuredVFX1.color.r, InjuredVFX1.color.g, InjuredVFX1.color.b, maxAlpha - alpha);
            InjuredVFX0.color = new Color(InjuredVFX0.color.r, InjuredVFX0.color.g, InjuredVFX0.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Final state with only InjuredVFX0 visible
        InjuredVFX1.color = new Color(InjuredVFX1.color.r, InjuredVFX1.color.g, InjuredVFX1.color.b, 0);
        InjuredVFX0.color = new Color(InjuredVFX0.color.r, InjuredVFX0.color.g, InjuredVFX0.color.b, maxAlpha);

        // Small delay before restarting the animation
        yield return new WaitForSeconds(resetDelay);

        // Restart the animation loop
        StartInjuredVFXAnimation((int)(50 * (1 - maxAlpha))); // Optional: simulate health update for demo purposes
    }

    public void StopInjuredVFXAnimation()
    {
        if (coroutineRunning)
        {
            StopAllCoroutines();
            // Disable all VFX images
            InjuredVFX0.gameObject.SetActive(false);
            InjuredVFX1.gameObject.SetActive(false);
            InjuredVFX2.gameObject.SetActive(false);
            coroutineRunning = false;
        }
    }
}
