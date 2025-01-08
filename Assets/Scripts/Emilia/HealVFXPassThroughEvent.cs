using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealVFXPassThroughEvent : MonoBehaviour
{
    [SerializeField] HealVFXHandler vFXHandler;
    public void CallSwirlAnim()
    {
        vFXHandler.StartSwirlAnim();
    }

    public void CallEndAnim()
    {
        vFXHandler.StartExitAnim();
    }
}
