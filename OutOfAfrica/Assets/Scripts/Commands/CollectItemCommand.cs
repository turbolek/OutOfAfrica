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
        var canPerform = _itemStack && _itemStack.Inventory.ContainsItem() > 0 &&
                         _unit.CanPickupItem();

        if (!canPerform)
        {
            Debug.Log($"{_unit.name} could not pick up resource from {_itemStack.name}");
        }

        return canPerform;
    }

    public override void Perform()
    {
        _itemStack.Inventory.RemoveItem();
        _unit.Inventory.AddResource();
        Debug.Log(
            $"{Unit.name} collected resource from {_itemStack.name}. Remaining amount: {_itemStack.Amount}");
    }
}