using UnityEngine;

public class ItemData : ScriptableObject
{
    [field: SerializeField] public ResourceType Resource { get; private set; }
    [field: SerializeField] public int SlotCapacity { get; private set; }
}