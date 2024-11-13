using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatTextDisappear : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            text.enabled = false;
        }
    }
}
