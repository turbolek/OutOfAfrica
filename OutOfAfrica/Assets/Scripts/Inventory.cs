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

    public bool IsOpen;

    private void Start()
    {
        var owner = GetComponent<Selectable>();
        if (owner != null)
        {
            Init(owner);
        }
    }

    public void Init(Selectable owner)
    {
        Owner = owner;
        foreach (var slot in ItemSlots)
        {
            slot.Init(this);
        }

        Initialized?.Invoke(this);
    }

    private void OnDestroy()
    {
        Destroyed?.Invoke(this);
    }

    public void AddItem(Item item)
    {
        var slot = GetSlotForAddingItemTo(item);
        if (slot != null)
        {
            slot.Item = item;
            slot.Increment();
        }
    }

    public void RemoveItem(Item item)
    {
        var slot = GetSlotForPickingUpItemFrom(item);
        if (slot != null)
        {
            slot.Decrement();
        }
    }

    public ItemSlot GetSlotForAddingItemTo(Item item)
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
                if (slot.CanFitItem(item) && (slot.Amount < bestSlot.Amount && slot.Item == bestSlot.Item ||
                                              (slot.Item == item && bestSlot.Item == null)))
                {
                    bestSlot = slot;
                }
            }
        }

        return bestSlot;
    }

    public ItemSlot GetSlotForPickingUpItemFrom(Item item)
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

    public bool ContainsItem(Item item)
    {
        return GetSlotForPickingUpItemFrom(item) != null;
    }

    public bool CanFitItem(Item item)
    {
        return GetSlotForAddingItemTo(item) != null;
    }

    public Item GetFirstItem()
    {
        foreach (var itemSlot in ItemSlots)
        {
            if (itemSlot.Item != null)
            {
                return itemSlot.Item;
            }
        }

        return null;
    }
}