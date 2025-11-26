using UnityEngine;

public class Man : MonoBehaviour
{
    [field: SerializeField]
    public Inventory Inventory { get; private set; }
    [field: SerializeField] public CombatAvatar CombatAvatarPrefab { get; private set; }
    [field: SerializeField] public Unit Unit { get; private set; }

    private void Start()
    {
        Inventory.Init();
    }
}
