using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text _resourceCount;
    [SerializeField] private Image _icon;

    public void DisplaySlot(ItemSlot itemSlot)
    {
        string resourceCount = string.Empty;
        Sprite resourceSprite = null;

        if (itemSlot.ItemData != null)
        {
            resourceCount = itemSlot.Amount.ToString();
            resourceSprite = itemSlot.ItemData.Sprite;
        }

        _resourceCount.text = resourceCount;
        _icon.sprite = resourceSprite;
        _icon.enabled = resourceSprite != null;
    }
}