using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{
    [SerializeField] BatProjectile batProjectile;

    private void Awake()
    {
        gameObject.transform.parent.GetComponentInChildren<BatProjectile>();
    }

    private void OnParticleCollision(GameObject other)
    {
        //batProjectile.DetectCollisionOnParticle(other);
    }
}
