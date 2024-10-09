using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapTrigger : MonoBehaviour
{
    [SerializeField] GameObject spikes;


    [SerializeField] bool active;
    [Header("Speeds as values from 0-1")]
    [SerializeField] float riseSpeed;
    [SerializeField] float lowerSpeed;
    [SerializeField] float activeHeight;

    private Vector3 inactivePosition;

    [SerializeField] float upTime;
    [SerializeField] float coolDown;

    [SerializeField] float timer;

    private void Start()
    {
        inactivePosition = spikes.transform.position - new Vector3(0, activeHeight, 0);
    }


    void Update()
    {
        if (active)
        {
            Vector3 targetPosition = new Vector3 (inactivePosition.x, inactivePosition.y + activeHeight, inactivePosition.z);
            spikes.transform.position = Vector3.Lerp(spikes.transform.position, targetPosition, riseSpeed);
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                active = false;
                timer = coolDown;
            }
        }
        else
        {
            spikes.transform.position = Vector3.Lerp(spikes.transform.position, inactivePosition, lowerSpeed);
            if (timer >0)
            {
                timer -= Time.deltaTime;
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {

            if (timer <= 0 && !active)
            {
                ActivateTrap();
            }
        }
    }

    public void ActivateTrap()
    {
        active = true;
        timer = upTime;
    }
}
