using System;
using UnityEngine;

[Serializable]
public class ResourceAmount
{
    [field: SerializeField] public ResourceType Resource { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
}