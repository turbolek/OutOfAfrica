using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampInteractionPopup : InteractionPopup
{
    public static event Action<Inventory> DayFinishRequested;

    [SerializeField] private Transform unitsInventoriesParent;
    [SerializeField] private InventoryView unitInventoryViewTemplate;
    [SerializeField] private InventoryView campInventoryView;
    [SerializeField] private CraftingView craftingView;
    [SerializeField] private Button finishDayButton;


    protected override void OnEnable()
    {
        base.OnEnable();
        finishDayButton.onClick.AddListener(OnFinishDayButtonClicked);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        finishDayButton.onClick.RemoveListener(OnFinishDayButtonClicked);
    }

    protected override void OnInit(PlayerUnitController unitGroup, Targetable targetable)
    {

        Inventory targetableInventory = null;

        var inventoryOwner = targetable.GetComponent<IInventoryOwner>();
        if (inventoryOwner != null)
        {
            targetableInventory = inventoryOwner.GetInventory();
        }

        campInventoryView.Init(targetableInventory);

        foreach (var groupMember in unitGroup.Members)
        {
            var groupMemberInventoryView = Instantiate(unitInventoryViewTemplate, unitsInventoriesParent);
            groupMemberInventoryView.Init(groupMember.Inventory);
            groupMemberInventoryView.gameObject.SetActive(true);
        }
        unitInventoryViewTemplate.gameObject.SetActive(false);


        //var craftingStation = targetable.GetComponent<CraftingStation>();
        //if (craftingStation != null)
        //{
        //    craftingView.Init(craftingStation);
        //}
    }

    private void OnFinishDayButtonClicked()
    {
        DayFinishRequested?.Invoke(campInventoryView.Inventory);
    }
}
