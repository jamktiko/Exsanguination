using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey;

    [SerializeField] public bool crosshairShown = true;
    RawImage rawImage;
    [SerializeField] Color color;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        color = rawImage.color;
    }

    void Update()
    {
        if (crosshairShown == true && Input.GetKeyDown(toggleKey))
        {
            HideCrosshair();
        }
        else if (crosshairShown == false && Input.GetKeyDown(toggleKey))
        {
            ShowCrosshair();
        }
    }

    public void ShowCrosshair()
    {
        rawImage.color = new Color(color.r, color.g, color.b, 1f);
        crosshairShown = true;
    }

    public void HideCrosshair()
    {
        rawImage.color = new Color(color.r, color.g, color.b, 0f);
        crosshairShown = false;
    }

    
}
