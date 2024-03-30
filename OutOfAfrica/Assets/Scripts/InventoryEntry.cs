using TMPro;
using UnityEngine;

public class InventoryEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceName;
    [SerializeField] private TMP_Text _resourceCount;

    public void DisplaySlot(ItemSlot itemSlot)
    {
        string resourceName = string.Empty;
        string resourceCount = string.Empty;

        if (itemSlot.ItemData != null)
        {
            resourceName = itemSlot.ItemData.name;
            resourceCount = itemSlot.Amount.ToString();
        }

        _resourceName.text = resourceName;
        _resourceCount.text = resourceCount;
    }
}