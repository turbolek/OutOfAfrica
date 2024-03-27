using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public Dictionary<ResourceType, int> Content { get; private set; } = new();

    [SerializeField] private ItemSlot[] _itemSlots;


    public void AddResource(ResourceType resourceType)
    {
        if (!Content.ContainsKey(resourceType))
        {
            Content.Add(resourceType, 0);
        }

        Content[resourceType] += 1;
    }

    public void RemoveItem(ResourceType resourceType)
    {
        if (Content.ContainsKey(resourceType))
        {
            Content[resourceType] = 0;
        }
    }

    public int GetResourceCount(ResourceType resourceType)
    {
        if (Content.ContainsKey(resourceType))
        {
            return Content[resourceType];
        }

        return 0;
    }

    public ItemSlot GetSlotForAddingItemTo(ItemData item)
    {
        ItemSlot bestSlot = null;

        foreach (var slot in _itemSlots)
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
                if (slot.CanFitItem(item) && slot.Amount <= bestSlot.Amount)
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

        foreach (var slot in _itemSlots)
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
}