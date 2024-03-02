using UnityEngine;

public class CollectResourceCommand : Command
{
    private ResourceStack _resourceStack;

    public CollectResourceCommand(PlayerUnitController unit, Targetable target) : base(unit, target)
    {
        _resourceStack = target.GetComponent<ResourceStack>();
    }

    public override bool Validate()
    {
        return _resourceStack && _resourceStack.Amount > 0;
    }

    public override void Perform()
    {
        _resourceStack.ChangeValue(-1);
        Debug.Log(
            $"{Unit.name} collected resource from {_resourceStack.name}. Remaining amount: {_resourceStack.Amount}");
    }
}