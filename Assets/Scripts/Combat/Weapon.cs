using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] private WeaponData weaponData;

    public WeaponData Data => weaponData;

    public float Damage
    {
        get
        {
            if (weaponData == null)
            {
                return 0f;
            }

            return weaponData.damage;
        }
    }

    public float AttackCooldown
    {
        get
        {
            if (weaponData == null)
            {
                return 0f;
            }

            return weaponData.attackCooldown;
        }
    }

    private void Awake()
    {
        if (weaponData == null)
        {
            Debug.LogError(
                "WeaponData is missing on " +
                gameObject.name + "!"
            );
        }
    }
}