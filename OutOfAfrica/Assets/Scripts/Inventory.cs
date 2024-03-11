using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [field: SerializeField] public Dictionary<ResourceType, int> Content { get; private set; } = new();

    [SerializeField] private int _inventoryCapacity = 10;

    public int RemainingInventoryCapacity
    {
        get
        {
            int cap = _inventoryCapacity;
            foreach (var resource in Content.Keys)
            {
                cap -= Content[resource];
            }

            return cap;
        }
    }

    public void AddResource(ResourceType resourceType)
    {
        if (!Content.ContainsKey(resourceType))
        {
            Content.Add(resourceType, 0);
        }

        Content[resourceType] += 1;
    }

    public void RemoveResource(ResourceType resourceType)
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
}