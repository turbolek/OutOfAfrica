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
    private TooltipRequester _tooltipRequester;

    private void Start()
    {
        _tooltipRequester = new TooltipRequester();
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

        if (itemSlot.Item != null && itemSlot.Item.Data != null)
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

    public void OnPointerEnter()
    {
        if (_canvasGroup.alpha <= 0f)
        {
            return;
        }

        var item = ItemSlot.Item;
        if (item != null && item.Data != null)
        {
            var title = item.Data.name;
            var message = string.Empty;

            foreach (var resourceAmount in item.Data.ResourceAmount)
            {
                message += $"{resourceAmount.Resource.name} : {resourceAmount.Amount.ToString()} \n";
            }

            if (item.Data.ToolCategory != null)
            {
                message += $"\nTool category: {item.Data.ToolCategory.name}\n";
            }

            if (item.Data.RequiredTool != null)
            {
                message += $"\nRequired tool: {item.Data.RequiredTool.name}\n";
            }

            _tooltipRequester.RequestShow(title, message);
        }
    }

    public void OnPointerExit()
    {
        _tooltipRequester.RequestHide();
    }
}