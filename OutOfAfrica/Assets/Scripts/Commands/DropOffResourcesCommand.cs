using System.Collections.Generic;

public class DropOffResourcesCommand : Command
{
    private DropOffZone _dropOffZone;
    private PlayerUnitController _unit;
    private bool _performed;

    public DropOffResourcesCommand(PlayerUnitController unit, Targetable target) : base(unit, target)
    {
        _dropOffZone = target.GetComponent<DropOffZone>();
        _unit = unit;
    }

    public override bool Validate()
    {
        return !_performed && _dropOffZone != null && _unit != null;
    }

    public override void Perform()
    {
        var inventoryCopy = new Dictionary<ResourceType, int>(_unit.Inventory);

        foreach (var inventoryEntry in inventoryCopy)
        {
            _dropOffZone.AddResource(inventoryEntry.Key, inventoryEntry.Value);
            _unit.DropResource(inventoryEntry.Key);
        }

        _performed = true;
    }
}