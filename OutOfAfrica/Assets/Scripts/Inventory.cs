using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Action OpenRequested;
    public Action CloseRequested;

    [field: SerializeField] public ItemSlot[] ItemSlots { get; private set; }

    public void AddItem(ItemData item)
    {
        var slot = GetSlotForAddingItemTo(item);
        if (slot != null)
        {
            slot.ItemData = item;
            slot.Increment();
        }
    }

    public void RemoveItem(ItemData item)
    {
        var slot = GetSlotForPickingUpItemFrom(item);
        if (slot != null)
        {
            slot.Decrement();
        }
    }

    public ItemSlot GetSlotForAddingItemTo(ItemData item)
    {
        ItemSlot bestSlot = null;

        foreach (var slot in ItemSlots)
        {
            if (bestSlot == null)
            {
                if (slot.CanFitItem(item))
                {
                    bestSlot = slot;
                }
            }
            else
            {
                if (slot.CanFitItem(item) && (slot.Amount < bestSlot.Amount && slot.ItemData == bestSlot.ItemData ||
                                              (slot.ItemData == item && bestSlot.ItemData == null)))
                {
                    bestSlot = slot;
                }
            }
        }

        return bestSlot;
    }

    public ItemSlot GetSlotForPickingUpItemFrom(ItemData item)
    {
        ItemSlot bestSlot = null;

        foreach (var slot in ItemSlots)
        {
            if (bestSlot == null)
            {
                if (slot.ContainsItem(item))
                {
                    bestSlot = slot;
                }
            }
            else
            {
                if (slot.ContainsItem(item) && slot.Amount <= bestSlot.Amount)
                {
                    bestSlot = slot;
                }
            }
        }

        return bestSlot;
    }

    public bool ContainsItem(ItemData item)
    {
        return GetSlotForPickingUpItemFrom(item) != null;
    }

    public bool CanFitItem(ItemData item)
    {
        return GetSlotForAddingItemTo(item) != null;
    }

    public ItemData GetFirstItem()
    {
        foreach (var itemSlot in ItemSlots)
        {
            if (itemSlot.ItemData != null)
            {
                return itemSlot.ItemData;
            }
        }

        return null;
    }

    public void Open()
    {
        OpenRequested?.Invoke();
    }

    public void Close()
    {
        CloseRequested?.Invoke();
    }
}