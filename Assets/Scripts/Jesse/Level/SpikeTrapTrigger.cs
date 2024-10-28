using System.Collections;
using UnityEngine;

public class SpikeTrapTrigger : MonoBehaviour
{
    [SerializeField] GameObject spikes;

    [Header("Speeds as values from 0-1")]
    [SerializeField] float riseSpeed;
    [SerializeField] float lowerSpeed;
    private Vector3 inactivePosition;
    [SerializeField] float inactiveYOffset;

    [SerializeField] float upTime;
    [SerializeField] float coolDown;

    [SerializeField] bool active;

    private void Awake()
    {
        Debug.Log(transform.root.transform.localScale.y);
        inactivePosition = spikes.transform.position + new Vector3(0, inactiveYOffset * transform.root.transform.localScale.y, 0);
        spikes.transform.position = inactivePosition;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if (active) { return; }
            StartCoroutine(Thrust());
        }
    }

    IEnumerator Thrust()
    {
        active = true;
        while (spikes.transform.position != transform.position)
        {
            spikes.transform.position = Vector3.MoveTowards(spikes.transform.position, transform.position, riseSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Spikes up");


        yield return new WaitForSeconds(upTime);

        while (spikes.transform.position != inactivePosition)
        {
            spikes.transform.position = Vector3.MoveTowards(spikes.transform.position, inactivePosition, lowerSpeed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Spikes down");

        yield return new WaitForSeconds(coolDown);
        active = false;
    }
}