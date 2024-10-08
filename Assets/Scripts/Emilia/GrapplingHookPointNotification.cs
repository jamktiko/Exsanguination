using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookPointNotification : MonoBehaviour
{
    private Light hookPointLight;
    private Light lastHookPointLight;
    private Ray ray;

    private bool isCoroutineRunning = false; // Flag to track coroutine state

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 25f);

        if (hit.collider != null)
        {
            Debug.DrawRay(gameObject.transform.position, ray.direction, Color.yellow);
            Debug.Log("ray hit: " + hit.collider.name);

            if (hit.collider.CompareTag("HookHitPoint"))
            {
                hookPointLight = hit.collider.GetComponentInChildren<Light>();
                if (lastHookPointLight == null)
                {
                    lastHookPointLight = hookPointLight;
                }
                else if (lastHookPointLight != hookPointLight)
                {
                    lastHookPointLight.enabled = false;
                    lastHookPointLight = hookPointLight;
                }

                if (hookPointLight != null)
                {
                    hookPointLight.enabled = true;
                }
            }
            else if (lastHookPointLight != null && !isCoroutineRunning)
            {
                StartCoroutine(HookPointLightLoop());
            }
        }
    }

    IEnumerator HookPointLightLoop()
    {
        isCoroutineRunning = true; // Set flag to indicate coroutine is running
        yield return new WaitForSeconds(2f);
        lastHookPointLight.enabled = false;
        lastHookPointLight = null; // Reset last hook point light to avoid issues
        isCoroutineRunning = false; // Reset flag
    }
}

