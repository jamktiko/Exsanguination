using UnityEngine;
using UnityEngine.UI;

public class GrappleCooldown : MonoBehaviour
{
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (image.fillAmount == 1)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
    }

    public void UpdateCooldown(float fillAmount)
    {
        image.fillAmount = fillAmount;
    }
}
