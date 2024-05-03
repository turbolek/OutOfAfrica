using UnityEngine;

public class CollectItemCommand : Command
{
    public ItemSlot SourceSlot { get; private set; }
    public ItemSlot TargetSlot { get; private set; }

    public CollectItemCommand(PlayerUnitController unit, Targetable target) : base(unit, target)
    {
    }

    public CollectItemCommand(PlayerUnitController unit, ItemSlot sourceSlot, ItemSlot targetSlot) :
        base(unit, sourceSlot, targetSlot)
    {
        Unit = unit;
        SourceSlot = sourceSlot;
        TargetSlot = targetSlot;
    }

    public override bool Validate()
    {
        var item = SourceSlot.Item;
        bool canPerform = item != null && TargetSlot.CanFitItem(item) && Unit.HasTool(item.Data.RequiredTool);
        return canPerform;
    }

    public override void Perform()
    {
        if (SourceSlot.Item != null && SourceSlot.Item.CollectionProgress >= 1f)
        {
            TargetSlot.Item = SourceSlot.Item;
            TargetSlot.Increment();
            SourceSlot.Decrement();
        }
        else
        {
            SourceSlot.Item.ChangeCollectionProgress(Time.deltaTime / SourceSlot.Item.Data.CollectionTime);
        }
    }

    public override float GetCooldown()
    {
        return 0f;
    }
}