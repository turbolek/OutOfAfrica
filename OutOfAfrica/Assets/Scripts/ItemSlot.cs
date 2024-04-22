using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ItemSlot
{
    public Action Modified;
    public ItemData InitialItem;
    public Item Item;
    public Inventory Inventory { get; private set; }

    [HideInInspector] //changed design for all items to have capacity == 1. Leaved the functionality in case design changes again to various capacities.
    public int Amount = 1;

    public void Init(Inventory inventory)
    {
        Inventory = inventory;
        Item = InitialItem != null ? new Item(InitialItem) : null;
    }

    public bool CanFitItem(Item item)
    {
        return Item == null || (Item == item && Amount < Item.Data.SlotCapacity);
    }

    public bool ContainsItem(Item item)
    {
        return Item == item && Amount > 0;
    }

    public void Increment()
    {
        Amount++;
        Modified?.Invoke();
    }

    public void Decrement()
    {
        Amount--;
        Amount = Mathf.Clamp(Amount, 0, Amount + 1);
        if (Amount <= 0)
        {
            Item = null;
        }

        Modified?.Invoke();
    }
}