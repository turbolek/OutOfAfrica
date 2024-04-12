using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data")]
public class ItemData : SerializedScriptableObject
{
    [field: SerializeField] public ResourceAmount[] ResourceAmount { get; private set; } = { };

    [HideInInspector]
    [field: SerializeField]
    public int SlotCapacity { get; private set; } = 1;

    [field: SerializeField] public Sprite Sprite { get; private set; }
}