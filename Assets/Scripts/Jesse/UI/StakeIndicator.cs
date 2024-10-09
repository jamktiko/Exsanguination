using UnityEngine;
using UnityEngine.UI;

public class StakeIndicator : MonoBehaviour
{
    Image image;

    [SerializeField] Color availableColor;
    [SerializeField] Color unavailableColor;

    void Awake()
    {
        image = GetComponent<Image>();
        StakeAvailable(true);
    }

    void StakeAvailable(bool boolean)
    {

        if (boolean)
        {
            image.color = availableColor;
        }
        else
        {
            image.color = unavailableColor;
        }
    }
}
