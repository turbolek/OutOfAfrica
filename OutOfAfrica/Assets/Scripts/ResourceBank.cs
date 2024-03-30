using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource Bank", menuName = "Resource Bank")]
public class ResourceBank : SerializedScriptableObject
{
    public Action Modified;

    [ShowInInspector] private Dictionary<ResourceType, int> _content = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> ContentCopy => new Dictionary<ResourceType, int>(_content);

    public void AddResource(ResourceType resource, int amount)
    {
        if (!_content.ContainsKey(resource))
        {
            _content.Add(resource, 0);
        }

        _content[resource] += amount;
        Modified?.Invoke();
    }

    public bool HasResource(ResourceType resource, int amount)
    {
        if (!_content.ContainsKey(resource))
        {
            _content.Add(resource, 0);
        }

        return _content[resource] >= amount;
    }

    public void RemoveResource(ResourceType resource, int amount)
    {
        if (!_content.ContainsKey(resource))
        {
            _content.Add(resource, 0);
        }

        _content[resource] -= amount;
        Modified?.Invoke();
    }
}