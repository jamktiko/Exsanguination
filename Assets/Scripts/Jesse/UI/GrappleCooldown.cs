using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GrappleCooldown : MonoBehaviour
{
    Image image;
   public float grappleCooldownTime;
    private bool isFinished;

    void Awake()
    {
        image = GetComponent<Image>();
        
    }

    private void Start()
    {
        image.fillAmount = 0;
    }

    void Update()
    {
        if (image.fillAmount == 0 && isFinished)
        {
            image.fillAmount = Mathf.Lerp(0,1, grappleCooldownTime);
        }
        if(image.fillAmount >= 1)
        {
            isFinished = false;
            StartCoroutine(ShowImageForSecond());
        }
       

    }

    IEnumerator ShowImageForSecond()
    {
        yield return new WaitForSeconds(1);
        image.fillAmount = 0;
    }

    public void SetGrappleCooldownTime(float fillAmount)
    {
        isFinished = true;
        grappleCooldownTime = fillAmount;
    }
}
