using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Character Data")]
    [SerializeField] private CharacterData characterData;

    public CharacterData Data => characterData;

    public float MaxHealth
    {
        get
        {
            if (characterData == null)
            {
                return 0f;
            }

            return characterData.maxHealth;
        }
    }

    private void Awake()
    {
        if (characterData == null)
        {
            Debug.LogError(
                "CharacterData is missing on CharacterStats!"
            );
        }
    }
}