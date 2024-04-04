using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private InventoryEntry _entryPrefab;
    [SerializeField] private Transform _entryParent;

    public Inventory Inventory { get; private set; }
    private Camera _camera;
    private List<InventoryEntry> _inventoryEntries = new();

    private void Start()
    {
        Hide();
    }

    private void OnDisable()
    {
        //InputController.SelectableHovered -= OnSelectableHovered;
    }

    public void Init(Inventory inventory, Camera camera)
    {
        _camera = camera;
        Inventory = inventory;
        DisplaySelectable(inventory.Owner);
        DisplayInventory(inventory);
    }

    public void Show()
    {
        transform.position = _camera.WorldToScreenPoint(Inventory.Owner.transform.position) + _offset;
        _canvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0f;
    }

    private void OnSelectableHovered(Selectable selectable)
    {
        if (selectable == null)
        {
            Hide();
            return;
        }

        for (int i = _inventoryEntries.Count - 1; i >= 0; i--)
        {
            Destroy(_inventoryEntries[i].gameObject);
        }

        _inventoryEntries.Clear();

        transform.position = _camera.WorldToScreenPoint(Inventory.transform.position) + _offset;
        Show();
        DisplaySelectable(selectable);

        Inventory inventory = selectable.GetComponent<Inventory>();
        if (inventory == null)
        {
            return;
        }

        DisplayInventory(inventory);
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
    }

    private void OnOpenRequest(Selectable owner)
    {
        if (owner == Inventory.Owner)
        {
            Show();
        }
    }

    private void OnCloseRequest(Selectable owner)
    {
        if (owner == Inventory.Owner)
        {
            Hide();
        }
    }
}