using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Selection Variable", menuName = "Variables/Selection")]
public class SelectionVariable : GenericVariable<List<PlayerUnitController>>
{
    public void Add(PlayerUnitController playerUnit)
    {
        var newValue = new List<PlayerUnitController>(Value);
        newValue.AddExclusive(playerUnit);
        Set(newValue);
    }

    public void Remove(PlayerUnitController playerUnit)
    {
        var newValue = new List<PlayerUnitController>(Value);
        newValue.Remove(playerUnit);
        Set(newValue);
    }

    public void Clear()
    {
        Set(new List<PlayerUnitController>());
    }

    public void Set(PlayerUnitController unit)
    {
        Set(new List<PlayerUnitController> { unit });
    }
}