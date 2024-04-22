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
    [SerializeField] private Slider _collectionSlider;

    public ItemSlot ItemSlot { get; private set; }
    public Inventory Inventory { get; private set; }

    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
        Item.CollectionProgressChanged += OnItemCollectionProgressChanged;
    }

    private void OnDestroy()
    {
        ItemSlot.Modified -= OnSlotModified;
        Item.CollectionProgressChanged -= OnItemCollectionProgressChanged;
    }

    public void Init(Inventory inventory, CanvasGroup canvasGroup)
    {
        Inventory = inventory;
        _canvasGroup = canvasGroup;
    }

    public bool IsVisible()
    {
        return gameObject.activeInHierarchy && _canvasGroup.alpha > 0f;
    }

    public void DisplaySlot(ItemSlot itemSlot)
    {
        ItemSlot = itemSlot;

        ItemSlot.Modified -= OnSlotModified;
        ItemSlot.Modified += OnSlotModified;

        string resourceCount = string.Empty;
        Sprite resourceSprite = null;

        if (itemSlot.Item != null)
        {
            resourceCount = itemSlot.Amount.ToString();
            resourceSprite = itemSlot.Item.Data.Sprite;
        }

        _resourceCount.text = resourceCount;
        _icon.sprite = resourceSprite;
        _icon.enabled = resourceSprite != null;
        DisplayCollectionProgress();
    }

    private void OnButtonClicked()
    {
        ButtonClicked?.Invoke(this);
    }

    private void OnSlotModified()
    {
        DisplaySlot(ItemSlot);
    }

    private void OnItemCollectionProgressChanged(Item item)
    {
        if (ItemSlot.ContainsItem(item))
        {
            DisplayCollectionProgress();
        }
    }

    private void DisplayCollectionProgress()
    {
        var item = ItemSlot.Item;

        if (item == null)
        {
            _collectionSlider.value = 0f;
        }
        else
        {
            _collectionSlider.value = item.CollectionProgress;
        }

        _collectionSlider.gameObject.SetActive(item != null && item.CollectionProgress < 1f);
    }
}