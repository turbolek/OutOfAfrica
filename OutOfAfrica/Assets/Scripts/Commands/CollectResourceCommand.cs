using UnityEngine;

public class CollectResourceCommand : Command
{
    private ResourceStack _resourceStack;
    private PlayerUnitController _unit;

    public CollectResourceCommand(PlayerUnitController unit, Targetable target) : base(unit, target)
    {
        _resourceStack = target.GetComponent<ResourceStack>();
        _unit = unit;
    }

    public override bool Validate()
    {
        var canPerform = _resourceStack && _resourceStack.Amount > 0 &&
                         _unit.CanPickupResource(_resourceStack.ResourceType);

        if (!canPerform)
        {
            Debug.Log($"{_unit.name} could not pick up resource from {_resourceStack.name}");
        }

        return canPerform;
    }

    public override void Perform()
    {
        _resourceStack.ChangeValue(-1);
        _unit.PickupResource(_resourceStack.ResourceType);
        Debug.Log(
            $"{Unit.name} collected resource from {_resourceStack.name}. Remaining amount: {_resourceStack.Amount}");
    }
}