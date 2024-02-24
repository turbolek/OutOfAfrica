using System.Collections.Generic;
using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(fileName = "Selection Variable", menuName = "Variables/Selection")]
    public class SelectionValueVariable : GenericValueVariable<List<PlayerUnitController>>
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

        public void Set(PlayerUnitController unit)
        {
            Set(new List<PlayerUnitController> { unit });
        }

        public override void Clear()
        {
            Set(new List<PlayerUnitController>());
        }
    }
}