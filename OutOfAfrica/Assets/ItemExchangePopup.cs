using UnityEngine;

public class ItemExchangePopup : InteractionPopup
{
    [SerializeField] private Transform unitsInventoriesParent;
    [SerializeField] private InventoryView unitInventoryViewTemplate;
    [SerializeField] private InventoryView targetableInventoryView;

    protected override void OnInit(PlayerUnitController unitGroup, Targetable targetable)
    {
        Inventory targetableInventory = null;

        var inventoryOwner = targetable.GetComponent<IInventoryOwner>();
        if (inventoryOwner != null)
        {
            targetableInventory = inventoryOwner.GetInventory();
        }

        foreach (var groupMember in unitGroup.Members)
        {
            var groupMemberInventoryView = Instantiate(unitInventoryViewTemplate, unitsInventoriesParent);
            groupMemberInventoryView.Init(groupMember.Inventory);
            groupMemberInventoryView.gameObject.SetActive(true);
        }

        targetableInventoryView.Init(targetableInventory);
        unitInventoryViewTemplate.gameObject.SetActive(false);
    }
}
