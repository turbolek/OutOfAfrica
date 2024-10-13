using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExchangePopup : InteractionPopup
{
    [SerializeField] private InventoryView unitInventoryView;
    [SerializeField] private InventoryView targetableInventoryView;

    protected override void OnInit(PlayerUnitController unit, Targetable targetable)
    {
        var unitInventory = unit.GetInventory();
        Inventory targetableInventory = null;

        var inventoryOwner = targetable.GetComponent<IInventoryOwner>();
        if (inventoryOwner != null)
        {
            targetableInventory = inventoryOwner.GetInventory();
        }

        unitInventoryView.Init(unitInventory);
        targetableInventoryView.Init(targetableInventory);
    }
}
