using System.Collections.Generic;

public class DropOffResourcesCommand : Command
{
    private DropOffZone _dropOffZone;
    private PlayerUnitController _unit;

    public DropOffResourcesCommand(PlayerUnitController unit, Targetable target) : base(unit, target)
    {
        _dropOffZone = target.GetComponent<DropOffZone>();
        _unit = unit;
    }

    public override bool Validate()
    {
        var item = _unit.GetInventory().GetFirstItem();
        return item != null && _dropOffZone != null && _unit != null;
    }

    public override void Perform()
    {
        var item = _unit.GetInventory().GetFirstItem();
        _unit.GetInventory().RemoveItem(item);
        //_dropOffZone.Bank.AddResource(item.Resource, 1);
    }

    public override float GetCooldown()
    {
        return 0f;
    }
}