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
        var inventoryCopy = new Dictionary<ResourceType, int>(_unit.Inventory.Content);

        foreach (var inventoryEntry in inventoryCopy)
        {
            for (int i = 0; i < inventoryEntry.Value; i++)
            {
                _dropOffZone.Inventory.AddResource(inventoryEntry.Key);
            }

            _unit.Inventory.RemoveResource(inventoryEntry.Key);
        }

        _performed = true;
    }
}