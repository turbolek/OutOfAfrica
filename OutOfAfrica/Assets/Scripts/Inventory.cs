using System;
using System.Collections.Generic;
using UnityEngine;
using Variables;

public class Inventory : MonoBehaviour
{
    public static Action<Inventory> Initialized;
    public static Action<Inventory> Destroyed;

    [field: SerializeField] public ItemSlot[] ItemSlots { get; private set; }
    [field: SerializeField] public int SortPriority;
    [field: SerializeField] public Vector3 ViewOffset { get; private set; }

    public Selectable Owner { get; private set; }
    public List<Inventory> ConnectedInventories { get; private set; } = new();

    private void Start()
    {
        Owner = GetComponent<Selectable>();
        Initialized?.Invoke(this);
    }

    private void OnDestroy()
    {
        Destroyed?.Invoke(this);
    }

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
}