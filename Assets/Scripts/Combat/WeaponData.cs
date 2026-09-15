using UnityEngine;

[CreateAssetMenu(
    fileName = "WeaponData",
    menuName = "Game/Weapon Data"
)]
public class WeaponData : ScriptableObject
{
    [Header("Identity")]
    public string weaponName;

    [Header("Combat")]
    public float damage = 25f;

    [Header("Attack")]
    public float attackCooldown = 0.8f;
}