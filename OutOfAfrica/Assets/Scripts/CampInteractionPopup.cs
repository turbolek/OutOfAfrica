using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampInteractionPopup : InteractionPopup
{
    [SerializeField] private InventoryView unitInventoryView;
    [SerializeField] private InventoryView campInventoryView;
    [SerializeField] private CraftingView craftingView;

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
        campInventoryView.Init(targetableInventory);

        var craftingStation = targetable.GetComponent<CraftingStation>();
        if (craftingStation != null)
        {
            craftingView.Init(craftingStation);
        }
    }
}
