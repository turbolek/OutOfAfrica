using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEntry : MonoBehaviour
{
    public static Action<InventoryEntry> ButtonClicked;

    [SerializeField] private TMP_Text _resourceCount;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;

    public ItemSlot ItemSlot { get; private set; }
    public Inventory Inventory { get; private set; }

    private void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnDestroy()
    {
        ItemSlot.Modified -= OnSlotModified;
    }

    public void Init(Inventory inventory)
    {
        Inventory = inventory;
    }

    public void DisplaySlot(ItemSlot itemSlot)
    {
        ItemSlot = itemSlot;

        ItemSlot.Modified -= OnSlotModified;
        ItemSlot.Modified += OnSlotModified;

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

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(this);
    }

    private void OnSlotModified()
    {
        DisplaySlot(ItemSlot);
    }
}