using UnityEngine;

[CreateAssetMenu(
    fileName = "CharacterData",
    menuName = "Game/Character Data"
)]
public class CharacterData : ScriptableObject
{
    [Header("Identity")]
    public string characterName;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

    [Header("Rotation")]
    public float rotationSpeed = 6f;

    [Header("Health")]
    public float maxHealth = 100f;
}