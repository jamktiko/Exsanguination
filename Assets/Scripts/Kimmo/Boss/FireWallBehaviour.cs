using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallBehaviour : MonoBehaviour
{
    public bool isGrowing;
    Vector3 scale;
    Vector3 horizontalScaleIncrease = new Vector3(0, 0, 0.1f);
    Vector3 verticalScaleDecrease = new Vector3(0, 0.1f, 0);
    [SerializeField] float fireWallLength;
    [SerializeField] float firewallHorizontalGrowthSpeed;
    [SerializeField] float firewallVerticalShrinkSpeed;
    [SerializeField] GameObject smoulderingLine;
    [SerializeField] Vector3 startingPosition;

    private void Start()
    {
        scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, scale.y, 0);
    }

    private void Update()
    {
        if (isGrowing)
        {
            if (transform.localScale.z < fireWallLength)
            {
                transform.localScale += horizontalScaleIncrease * firewallHorizontalGrowthSpeed * Time.deltaTime;
            }
            else
            {
                StartCoroutine(WaitBeforeRemovingFireWall());
            }
        }
    }

    IEnumerator WaitBeforeRemovingFireWall()
    {
        isGrowing = false;

        yield return new WaitForSeconds(5);

        StartCoroutine(RemoveFireWall());
    }

    IEnumerator RemoveFireWall()
    {
        while (transform.localScale.y > 0)
        {

            transform.localScale -= verticalScaleDecrease * firewallVerticalShrinkSpeed * Time.deltaTime;
            yield return null;
        }

        smoulderingLine.transform.position = startingPosition;
        transform.localScale = new Vector3(scale.x, 1, 0);
    }
}
