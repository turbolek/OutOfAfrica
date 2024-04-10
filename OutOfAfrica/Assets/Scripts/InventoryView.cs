using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public static Action<RectTransform, Vector2> FixSubscribeRequested;
    public static Action<RectTransform> FixUnsubscribeRequested;

    [SerializeField] private TMP_Text _title;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private InventoryEntry _entryPrefab;
    [SerializeField] private Transform _entryParent;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private int _maxColumnsCount;

    public Inventory Inventory { get; private set; }
    private Camera _camera;
    private List<InventoryEntry> _inventoryEntries = new();
    private RectTransform _rectTransform;

    private bool _isShown = true;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _isShown = true;
        Hide();
    }

    private void OnDisable()
    {
        FixUnsubscribeRequested?.Invoke(_rectTransform);
    }

    public void Init(Inventory inventory, Camera camera)
    {
        _camera = camera;
        Inventory = inventory;
        name = $"{inventory.Owner.name} inventory view";
        DisplaySelectable(inventory.Owner);
        DisplayInventory(inventory);
    }

    public void Show()
    {
        if (_isShown)
        {
            return;
        }

        var position = _camera.WorldToScreenPoint(Inventory.Owner.transform.position) + _offset;
        transform.position = position;
        FixSubscribeRequested?.Invoke(_rectTransform, position);
        _canvasGroup.alpha = 1f;
        _isShown = true;
    }

    public void Hide()
    {
        if (!_isShown)
        {
            return;
        }

        _canvasGroup.alpha = 0f;
        FixUnsubscribeRequested?.Invoke(_rectTransform);
        _isShown = false;
    }

    private void DisplaySelectable(Selectable selectable)
    {
        _title.text = selectable.name;
    }

    private void DisplayInventory(Inventory inventory)
    {
        for (int i = _inventoryEntries.Count - 1; i >= 0; i--)
        {
            Destroy(_inventoryEntries[i].gameObject);
        }

        _inventoryEntries.Clear();

        foreach (var itemSlot in inventory.ItemSlots)
        {
            InventoryEntry entry = Instantiate(_entryPrefab, _entryParent);
            entry.DisplaySlot(itemSlot);
            _inventoryEntries.Add(entry);
        }

        _gridLayoutGroup.constraintCount = Math.Clamp(_inventoryEntries.Count, 0, _maxColumnsCount);
    }
}