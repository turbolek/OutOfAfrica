using System;
using UnityEngine;

namespace Variables
{
    public abstract class GenericValueVariable<T> : GenericVariable
    {
        [field: SerializeField] public T Value { get; private set; }
        [HideInInspector] public Action<(T newValue, T oldValue)> Modified;

        public void Set(T value)
        {
            var oldValue = Value;
            Value = value;
            Modified?.Invoke((value, oldValue));
        }

        public override void Clear()
        {
            Set(default);
        }
    }
}