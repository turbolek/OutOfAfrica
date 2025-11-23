using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
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
    //private Vector3 _position => _camera.WorldToScreenPoint(Inventory.Owner.transform.position) + _offset;
    private OverlapFixRequester _overlapFixRequester = new OverlapFixRequester();


    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        //_isShown = true;
        //Hide();
    }

    private void Update()
    {
        //if (_isShown)
        //{
        //    _overlapFixRequester.RequestFixSubscribe(_rectTransform, _position);
        //}
    }

    private void OnDisable()
    {
        _overlapFixRequester.RequestFixUnsubscribe(_rectTransform);
    }

    public void Init(Inventory inventory)
    {
        if (inventory == null)
        {
            return;
        }

        Inventory = inventory;
        name = $"{inventory.name} inventory view";
        DisplaySelectable(inventory.Owner);
        DisplayInventory(inventory);
    }

    //public void Show()
    //{
    //    if (_isShown)
    //    {
    //        return;
    //    }

    //    transform.position = _position;
    //    _canvasGroup.alpha = 1f;
    //    _isShown = true;
    //    //_overlapFixRequester.RequestFixSubscribe(_rectTransform, _position);
    //    Inventory.IsOpen = true;
    //}

    //public void Hide()
    //{
    //    if (!_isShown)
    //    {
    //        return;
    //    }

    //    _canvasGroup.alpha = 0f;
    //    _overlapFixRequester.RequestFixUnsubscribe(_rectTransform);
    //    _isShown = false;
    //    Inventory.IsOpen = false;
    //}

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
            entry.Init(Inventory, _canvasGroup);
            entry.DisplaySlot(itemSlot);
            _inventoryEntries.Add(entry);
        }

        _gridLayoutGroup.constraintCount = Math.Clamp(_inventoryEntries.Count, 0, _maxColumnsCount);
    }
}