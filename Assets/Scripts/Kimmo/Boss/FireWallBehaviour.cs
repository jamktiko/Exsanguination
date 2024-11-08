using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWallBehaviour : MonoBehaviour
{
    public bool isCreated;
    Vector3 scale;
    Vector3 scaleIncrease = new Vector3(0, 0, 0.1f);
    [SerializeField] float scaleIncreaseSpeed;

    private void Start()
    {
        scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, scale.y, 0);
    }

    private void Update()
    {
        if (isCreated)
        {
            if (transform.localScale.z < 1)
            {
                transform.localScale += scaleIncrease * scaleIncreaseSpeed * Time.deltaTime;
            }
        }
    }
}
