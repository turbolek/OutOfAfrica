using UnityEngine;

[CreateAssetMenu(fileName = "Selectable Type", menuName = "Selectable Type")]

public class SelectableType : ScriptableObject
{
    [field: SerializeField] public int Priority { get; private set; }
}