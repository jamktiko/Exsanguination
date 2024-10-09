using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AapoAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource introSource;  // AudioSource for the intro clip
    [SerializeField] private AudioSource loopSource;   // AudioSource for the loop clip

    private void Start()
    {
        // Start playing the intro
        introSource.Play();

        // Start coroutine to switch to the loop after the intro ends
        StartCoroutine(PlayLoopAfterIntro());
    }

    private IEnumerator PlayLoopAfterIntro()
    {
        // Wait for the intro to finish playing
        yield return new WaitForSeconds(5.8f);

        // Start playing the loop
        loopSource.Play();

        // Set the loop to true so it continues looping indefinitely
        loopSource.loop = true;
    }
}
