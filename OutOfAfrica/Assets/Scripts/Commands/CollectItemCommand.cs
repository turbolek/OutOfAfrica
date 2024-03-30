using UnityEngine;

public class CollectItemCommand : Command
{
    private ItemStack _itemStack;
    private PlayerUnitController _unit;

    public CollectItemCommand(PlayerUnitController unit, Targetable target) : base(unit, target)
    {
        _itemStack = target.GetComponent<ItemStack>();
        _unit = unit;
    }

    public override bool Validate()
    {
        //TODO How to determine what item to pick up? Context menu?
        var item = _itemStack.Inventory.GetFirstItem();
        var canPerform = item != null && _itemStack && _itemStack.Inventory.ContainsItem(item) &&
                         _unit.CanPickupItem(item);

        if (!canPerform)
        {
            Debug.Log($"{_unit.name} could not pick up resource from {_itemStack.name}");
        }

        return canPerform;
    }

    public override void Perform()
    {
        var item = _itemStack.Inventory.GetFirstItem();

        _itemStack.Inventory.RemoveItem(item);
        _unit.Inventory.AddItem(item);
    }
}