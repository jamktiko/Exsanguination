using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletPatterns : MonoBehaviour
{ /// <summary>
/// extraPatterns 0 = normal executepattern with variables
/// extraPatterns 1 = Wave pattern
/// extraPatterns 2 = ConvergingWaves pattern
/// extraPatterns 3 = RandomSpread pattern
/// extraPatterns 4 = SpiralWithVariance pattern
/// </summary>
    public BulletHellPattern bulletHellPattern;
    public int extraPatterns;
    // Start is called before the first frame update
    void Start()
    {
        if(extraPatterns == 0)
        {
            bulletHellPattern.ExecutePattern(transform);
        }

        if (extraPatterns == 1)
        {
            bulletHellPattern.ExecuteWavePattern(transform);
        }
        if (extraPatterns == 2)
        {
            bulletHellPattern.ExecuteConvergingWavesPattern(transform);
        }
        if (extraPatterns == 3)
        {
            bulletHellPattern.ExecuteRandomSpreadPattern(transform);
        }

        if (extraPatterns == 4)
        {
            bulletHellPattern.ExecuteSpiralWithVariancePattern(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
