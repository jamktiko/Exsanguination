using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialTrigger : MonoBehaviour
{
    TextMeshProUGUI tutorialText;
    string thisName;

    private void Awake()
    {
        thisName = gameObject.name;
        tutorialText = GameObject.Find(thisName + "UI").GetComponent<TextMeshProUGUI>();
        tutorialText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            tutorialText.enabled = true;
            Debug.Log("Player triggered a tutorial.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            tutorialText.enabled = false;
            Debug.Log("Player triggered a tutorial.");
        }
    }
}
