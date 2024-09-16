using UnityEngine;

public class DangerScript : MonoBehaviour
{
    [SerializeField] LayerMask deathBoxLayer;
    [SerializeField] DeathScript deathScript;

    void Start()
    {
    }


    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DeathBox") 
        {
            deathScript.Die();
        }
    }
}
