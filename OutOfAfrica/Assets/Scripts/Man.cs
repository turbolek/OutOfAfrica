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

    private void OnEnable()
    {
        Inventory.Modified += OnInventoyModified;
    }

    private void Start()
    {
        Inventory.Init();
        Unit.Died += OnDied;

        OnInventoyModified(Inventory);
    }

    private void OnDisable()
    {
        Inventory.Modified -= OnInventoyModified;
    }

    private void OnDestroy()
    {
        Unit.Died -= OnDied;
    }

    private void OnDied()
    {
        Died?.Invoke(this);
    }

    private void OnInventoyModified(Inventory inventory)
    {
        ItemData bestWeapon = null;

        foreach (var slot in inventory.ItemSlots)
        {
            var item = slot.Item;

            if (item == null)
                continue;

            if (bestWeapon == null || item.Data.Strength > bestWeapon.Strength)
            {
                bestWeapon = item.Data;
            }
        }

        Unit.Weapon = bestWeapon;
    }
}
