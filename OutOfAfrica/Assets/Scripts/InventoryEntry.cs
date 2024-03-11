using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceName;
    [SerializeField] private TMP_Text _resourceCount;

    public void DisplayResource(ResourceType type, int count)
    {
        _resourceName.text = type.name;
        _resourceCount.text = count.ToString();
    }
}