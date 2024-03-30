using System;
using TMPro;
using UnityEngine;

public class ResourcePanel : MonoBehaviour
{
    [SerializeField] private ResourceBank _bank;

    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private TMP_Text _resourceName;
    [SerializeField] private TMP_Text _resourceCount;

    private void OnEnable()
    {
        _bank.Modified += DisplayValue;
        DisplayValue();
    }

    private void OnDisable()
    {
        _bank.Modified -= DisplayValue;
    }

    private void DisplayValue()
    {
        _resourceName.text = _resourceType.name;
        var count = _bank.ContentCopy.ContainsKey(_resourceType) ? _bank.ContentCopy[_resourceType] : 0;
        _resourceCount.text = count.ToString();
    }
}