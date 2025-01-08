using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealVFXHandler : MonoBehaviour
{
    [SerializeField] private Animator startHealCycleAnimation;
    [SerializeField] private Animator healCycleSwirlAnimation;
    [SerializeField] private Animator healCycleEndAnimation;

    private void Awake()
    {
        startHealCycleAnimation.gameObject.SetActive(false);
        healCycleSwirlAnimation.gameObject.SetActive(false);
        healCycleEndAnimation.gameObject.SetActive(false);
    }

    public void StartHealAnimation()
    {
        startHealCycleAnimation.gameObject.SetActive(true);
        startHealCycleAnimation.Play("StartHealVFXAnim");
    }

    public void StartSwirlAnim()
    {
        healCycleEndAnimation.gameObject.SetActive(true);
        healCycleEndAnimation.Play("HealSwirlAnim");
    }

    public void StartExitAnim()
    {
        healCycleEndAnimation.gameObject.SetActive(true);
        healCycleEndAnimation.Play("EndHealAnim");
    }
}
