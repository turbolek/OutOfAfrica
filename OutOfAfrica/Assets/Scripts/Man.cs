using System;
using UnityEngine;

public class Man : MonoBehaviour
{
    public static Action<Man> Died;
    [field: SerializeField]
    public Inventory Inventory { get; private set; }
    [field: SerializeField] public CombatAvatar CombatAvatarPrefab { get; private set; }
    [field: SerializeField] public Unit Unit { get; private set; }

    public PlayerUnitController Group { get; set; }

    private void Start()
    {
        Inventory.Init();
        Unit.Died += OnDied;
    }

    private void OnDestroy()
    {
        Unit.Died -= OnDied;
    }

    private void OnDied()
    {
        Died?.Invoke(this);
    }
}
