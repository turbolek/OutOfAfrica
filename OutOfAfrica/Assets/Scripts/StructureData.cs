using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Structure Data", menuName = "Structure Data")]
public class StructureData : SerializedScriptableObject, IRecipeProduct
{
    [field: SerializeField] public Sprite Sprite { get; private set; }

    public Sprite Icon => Sprite;
    public string Name => name;

    [field: SerializeField] public StructureGhost StructureGhostPrefab { get; private set; }
}