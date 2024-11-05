using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealVFXPassThroughEvent : MonoBehaviour
{
    [SerializeField] HealVFXHandler vFXHandler;
    public void CallSwirlAnim()
    {
        vFXHandler.StartSwirlAnim();
        gameObject.SetActive(false);
    }

    public void CallEndAnim()
    {
        vFXHandler.StartExitAnim();
        gameObject.SetActive(false);
    }
}
