using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeResourcePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceLabel;
    [SerializeField] private TMP_Text _resourceName;

    private Inventory _inputInventory;
    private ResourceAmount _requiredResource;
    private bool _initialzied;

    private void Update()
    {
        if (!_initialzied)
        {
            return;
        }

        _resourceName.text = $"{_requiredResource.Resource.name}: ";
        _resourceLabel.text = $"{GetResourceAmount()}/{_requiredResource.Amount}";
    }

    public void Init(ResourceAmount requiredResource, Inventory inputInventory)
    {
        _requiredResource = requiredResource;
        _inputInventory = inputInventory;
        _initialzied = true;
    }

    public bool HasResources()
    {
        if (GetResourceAmount() < _requiredResource.Amount)
        {
            return false;
        }

        return true;
    }

    private int GetResourceAmount()
    {
        int amount = 0;

        foreach (var slot in _inputInventory.ItemSlots)
        {
            if (slot.Item != null)
            {
                foreach (var resourceAmount in slot.Item.Data.ResourceAmount)
                {
                    if (resourceAmount.Resource == _requiredResource.Resource)
                    {
                        amount += resourceAmount.Amount;
                    }
                }
            }
        }

        return amount;
    }
}