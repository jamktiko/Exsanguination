using UnityEngine;
public class BossWalkAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip[] footSteps; //right is first and left is second
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayRightFootSfx()
    {
        float randomPitch = Random.Range(0.95f, 1.05f);
        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(footSteps[0]);
    }

    public void PlayLeftFootSfx()
    {
        float randomPitch = Random.Range(0.95f, 1.05f);
        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(footSteps[1]);
    }
}
