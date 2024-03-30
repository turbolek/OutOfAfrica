using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public ResourceType Resource { get; private set; }
    [field: SerializeField] public int SlotCapacity { get; private set; }
}