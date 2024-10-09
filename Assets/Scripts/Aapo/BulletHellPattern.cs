using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletHellPattern", menuName = "ScriptableObjects/BulletHellPattern", order = 1)]
public class BulletHellPattern : ScriptableObject
{
    [System.Serializable]
    public class BulletPattern
    {
        public float speed;
        public float delayBetweenShots;
        public int numberOfBullets;
        public float angleBetweenBullets;
        public GameObject bulletPrefab;
    }

    public List<BulletPattern> patterns;

    public void ExecutePattern(Transform origin)
    {
        foreach (var pattern in patterns)
        {
            origin.GetComponent<MonoBehaviour>().StartCoroutine(ShootBullets(pattern, origin));
        }
    }

    public void ExecuteWavePattern(Transform origin)
    {
        foreach (var pattern in patterns)
        {
            origin.GetComponent<MonoBehaviour>().StartCoroutine(WavePattern(pattern, origin));
        }
    }
    public void ExecuteRandomSpreadPattern(Transform origin)
    {
        foreach (var pattern in patterns)
        {
            origin.GetComponent<MonoBehaviour>().StartCoroutine(RandomSpread(pattern, origin));
        }
    }

    public void ExecuteConvergingWavesPattern(Transform origin)
    {
        foreach (var pattern in patterns)
        {
            origin.GetComponent<MonoBehaviour>().StartCoroutine(ConvergingWaves(pattern, origin));
        }
    }

    public void ExecuteSpiralWithVariancePattern(Transform origin)
    {
        foreach (var pattern in patterns)
        {
            origin.GetComponent<MonoBehaviour>().StartCoroutine(SpiralWithVariance(pattern, origin));
        }
    }

    private IEnumerator<WaitForSeconds> ShootBullets(BulletPattern pattern, Transform origin)
    {
        for (int i = 0; i < pattern.numberOfBullets; i++)
        {
            float angle = i * pattern.angleBetweenBullets;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            GameObject bullet = Instantiate(pattern.bulletPrefab, origin.position, rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * pattern.speed;
            yield return new WaitForSeconds(pattern.delayBetweenShots);
        }
    }

    public IEnumerator<WaitForSeconds> WavePattern(BulletPattern pattern, Transform origin)
    {
        for (int i = 0; i < pattern.numberOfBullets; i++)
        {
            float angle = Mathf.Sin(i * pattern.angleBetweenBullets) * 45;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            GameObject bullet = Instantiate(pattern.bulletPrefab, origin.position, rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * pattern.speed;
            yield return new WaitForSeconds(pattern.delayBetweenShots);
        }
    }
    public IEnumerator<WaitForSeconds> RandomSpread(BulletPattern pattern, Transform origin)
    {
        for (int i = 0; i < pattern.numberOfBullets; i++)
        {
            float angle = Random.Range(0, 360);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            GameObject bullet = Instantiate(pattern.bulletPrefab, origin.position, rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * pattern.speed;
            yield return new WaitForSeconds(pattern.delayBetweenShots);
        }
    }

    public IEnumerator<WaitForSeconds> ConvergingWaves(BulletPattern pattern, Transform origin)
    {
        for (int i = 0; i < pattern.numberOfBullets / 2; i++)
        {
            float angleLeft = Mathf.Sin(i * pattern.angleBetweenBullets) * 45;
            Quaternion rotationLeft = Quaternion.Euler(new Vector3(0, angleLeft, 0));
            GameObject bulletLeft = Instantiate(pattern.bulletPrefab, origin.position, rotationLeft);
            bulletLeft.GetComponent<Rigidbody>().velocity = bulletLeft.transform.forward * pattern.speed;

            float angleRight = Mathf.Sin(i * pattern.angleBetweenBullets) * -45;
            Quaternion rotationRight = Quaternion.Euler(new Vector3(0, angleRight, 0));
            GameObject bulletRight = Instantiate(pattern.bulletPrefab, origin.position, rotationRight);
            bulletRight.GetComponent<Rigidbody>().velocity = bulletRight.transform.forward * pattern.speed;

            yield return new WaitForSeconds(pattern.delayBetweenShots);
        }
    }
    public IEnumerator<WaitForSeconds> SpiralWithVariance(BulletPattern pattern, Transform origin)
    {
        for (int i = 0; i < pattern.numberOfBullets; i++)
        {
            float baseAngle = i * pattern.angleBetweenBullets;
            float variance = Random.Range(-5, 5);
            float angle = baseAngle + variance;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angle, 0));
            GameObject bullet = Instantiate(pattern.bulletPrefab, origin.position, rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * pattern.speed;
            yield return new WaitForSeconds(pattern.delayBetweenShots);
        }
    }

}
