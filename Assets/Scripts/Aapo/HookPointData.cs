using UnityEngine;

public class HookPointData : MonoBehaviour
{
    public GameObject unAimedObject;
    public GameObject aimedObject;

    private void Start()
    {
        unAimedObject.SetActive(true);
        aimedObject.SetActive(false);
    }
}
