using UnityEngine;
using System.Collections;

public class AudioCrossfade : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public float fadeDuration = 2f;  // Duration of the crossfade in seconds

    public void Crossfade()
    {
        StartCoroutine(CrossfadeCoroutine());
    }

    private IEnumerator CrossfadeCoroutine()
    {
        float time = 0f;

        // Store the starting volumes
        float startVolume1 = audioSource1.volume;
        float startVolume2 = audioSource2.volume;

        // Play the second audio if it's not already playing
        if (!audioSource2.isPlaying)
        {
            audioSource2.Play();
        }

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            // Gradually lower volume of audioSource1 and raise volume of audioSource2
            audioSource1.volume = Mathf.Lerp(startVolume1, 0, t);
            audioSource2.volume = Mathf.Lerp(startVolume2, 1, t);

            yield return null;
        }

        // Make sure volumes are set to end values
        audioSource1.volume = 0;
        audioSource2.volume = 1;

        // Optionally, stop the first track if needed
        audioSource1.Stop();
    }
}
