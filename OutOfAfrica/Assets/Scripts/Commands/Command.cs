public abstract class Command
{
    public PlayerUnitController Unit;
    public Targetable Target;

    public Command(PlayerUnitController unit, Targetable target)
    {
        Unit = unit;
        Target = target;
    }

    public Command(PlayerUnitController unit, params object[] parameters)
    {
        
    }

    public abstract bool Validate();
    public abstract void Perform();
    public abstract float GetCooldown();
}