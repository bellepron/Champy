using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Objects/Level/New Level Definition")]
public class LevelDefinition : ScriptableObject
{
    [Header("Player")]
    [Space]
    public float speed;

    [Header("Level Definitions")]
    [Space]
    public GameObject levelPrefab;
}