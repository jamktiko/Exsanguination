using UnityEngine;

[CreateAssetMenu(fileName = "new weapon", menuName = "Item/Weapon")]
public class Weapon : ScriptableObject
{
    public int damage;
    public float swingSpeed;
    public string weaponName;
    public string description;
    public GameObject prefab;

}
