using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ItemSlot
{
    public Action Modified;
    public ItemData ItemData;

    [HideInInspector] //changed design for all items to have capacity == 1. Leaved the functionality in case design changes again to various capacities.
    public int Amount = 1;

    public bool CanFitItem(ItemData item)
    {
        return ItemData == null || (ItemData == item && Amount < ItemData.SlotCapacity);
    }

    public bool ContainsItem(ItemData item)
    {
        return ItemData == item && Amount > 0;
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
            ItemData = null;
        }

        Modified?.Invoke();
    }
}