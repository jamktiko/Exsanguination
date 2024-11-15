using UnityEngine;

public class CutsceneSkip : MonoBehaviour
{
    public static CutsceneSkip Instance;
    public bool hasSeenCutscene = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps this object across scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance if one already exists
        }
    }

    public void MarkCutsceneAsSeen()
    {
        hasSeenCutscene = true;
    }
}
