using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static float totalTimeSpent;
    private void Update()
    {
        totalTimeSpent += Time.deltaTime;
        
    }
}
