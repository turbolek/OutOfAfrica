using System.Collections.Generic;
using UnityEngine;

namespace Variables
{
    [CreateAssetMenu(fileName = "Selection Variable", menuName = "Variables/Selection")]
    public class SelectionValueVariable : GenericValueVariable<List<Selectable>>
    {
        public void Add(Selectable selectable)
        {
            var newValue = new List<Selectable>(Value);
            newValue.AddExclusive(selectable);
            Set(newValue);
        }

        public void Remove(Selectable selectable)
        {
            var newValue = new List<Selectable>(Value);
            newValue.Remove(selectable);
            Set(newValue);
        }

        public void Set(Selectable selectable)
        {
            Set(new List<Selectable> { selectable });
        }

        public override void Clear()
        {
            Set(new List<Selectable>());
        }
    }
}