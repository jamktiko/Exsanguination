using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookPointNotification : MonoBehaviour
{
    private Light hookPointLight;
    private Light lastHookPointLight;
    private Ray ray;
    private float lightEnabledTimer = 2f;
    private float defaultLightAmount = 0.5f;
    private float highlightedLightAmount = 5f;


    private bool isCoroutineRunning = false; // Flag to track coroutine state

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 25f);

        if (hit.collider != null)
        {
            
            if (hit.collider.CompareTag("HookHitPoint"))
            {
                Debug.Log("hit light");
                hookPointLight = hit.collider.GetComponentInChildren<Light>();
                if (lastHookPointLight == null)
                {
                    lastHookPointLight = hookPointLight;
                }
                else if (lastHookPointLight != hookPointLight)
                {
                    lastHookPointLight.intensity = defaultLightAmount;
                    lastHookPointLight = hookPointLight;
                    Debug.Log("looked away from light");
                }

                if (hookPointLight != null)
                {
                    hookPointLight.intensity = highlightedLightAmount;
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
        yield return new WaitForSeconds(lightEnabledTimer);
        lastHookPointLight.intensity = defaultLightAmount;
        lastHookPointLight = null; // Reset last hook point light to avoid issues
        isCoroutineRunning = false; // Reset flag
    }

}

