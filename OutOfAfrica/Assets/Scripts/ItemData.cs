using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data")]
public class ItemData : SerializedScriptableObject, IRecipeProduct
{
    [field: SerializeField] public ResourceAmount[] ResourceAmount { get; private set; } = { };

    [HideInInspector]
    [field: SerializeField]
    public int SlotCapacity { get; private set; } = 1;

    [field: SerializeField] public float CollectionTime { get; private set; }

    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public ToolCategory ToolCategory { get; private set; }
    [field: SerializeField] public ToolCategory RequiredTool { get; private set; }
    public Sprite Icon => Sprite;
    public string Name => name;
}