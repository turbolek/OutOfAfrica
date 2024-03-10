using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffZone : MonoBehaviour
{
    private Dictionary<ResourceType, int> _inventory = new(); //TODO make a separate component

    public void AddResource(ResourceType resourceType, int amount)
    {
        if (!_inventory.ContainsKey(resourceType))
        {
            _inventory.Add(resourceType, 0);
        }

        _inventory[resourceType] += amount;

        Debug.Log($"Added {amount} {resourceType} to {name}");
    }
}