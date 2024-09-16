using EmiliaScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] Button[] buttonArray = new Button[4];

    [SerializeField] PlayerHealthManager playerHealthManager;


    void Start()
    {
        buttonArray[0].onClick.AddListener(HealButton);
        buttonArray[1].onClick.AddListener(DamageButton);
        //buttonArray[2].onClick.AddListener(IncreaseMaxHealthButton);
        //buttonArray[3].onClick.AddListener(DecreaseMaxHealthButton);
    }


    void Update()
    {
        
    }

    void HealButton()
    {
        playerHealthManager.UpdatePlayerHealth(10);
    }

    void DamageButton()
    {
        playerHealthManager.UpdatePlayerHealth(-10);
    }
}
