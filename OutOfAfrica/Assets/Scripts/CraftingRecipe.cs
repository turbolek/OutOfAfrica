using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "Crafting Recipe")]

public class CraftingRecipe : SerializedScriptableObject
{
    [field: SerializeField] public ResourceAmount[] RequiredResources { get; private set; } = { };
    [field: SerializeField] public ItemData Product { get; private set; }
}