using System;
using UnityEngine;

public abstract class GenericVariable<T> : ScriptableObject
{
    public T Value { get; private set; }
    public Action<(T newValue, T oldValue)> Modified;

    public void Set(T value)
    {
        var oldValue = Value;
        Value = value;
        Modified?.Invoke((value, oldValue));
    }
}