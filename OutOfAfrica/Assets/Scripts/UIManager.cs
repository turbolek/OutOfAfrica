using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Variables;

public class UIManager : MonoBehaviour
{
    public class InventoryConnection
    {
        public List<Inventory> Inventories = new();

        public InventoryConnection(Inventory inventory1, Inventory inventory2)
        {
            Inventories.Add(inventory1);
            Inventories.Add(inventory2);
        }
    }

    [SerializeField] private InventoryView _inventoryViewPrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectionValueVariable _selectionValueVariable;
    [SerializeField] private Transform _mainCanvasTransform;
    [SerializeField] private TransformVariable _mainCanvasVariable;
    private List<InventoryView> _inventoryViews = new();
    private List<InventoryConnection> _inventoryConnections = new();

    private void OnEnable()
    {
        Targetable.InteractionPopupRequest += OnInteractionPopupRequested;
        Inventory.Initialized += OnInventoryInitialized;
        Inventory.Destroyed += OnInventoryDestroyed;
        PlayerUnitController.ConnectInventoriesRequest += OnConnectInventoriesRequest;
        PlayerUnitController.DisconnectInventoriesRequest += OnDisconnectInventoriesRequest;
        _selectionValueVariable.Modified += OnSelectionModified;

        _mainCanvasVariable.Set(_mainCanvasTransform);
    }

    private void Update()
    {
        RefreshInventoryViews();
    }

    private void OnDisable()
    {
        Targetable.InteractionPopupRequest -= OnInteractionPopupRequested;
        Inventory.Initialized -= OnInventoryInitialized;
        Inventory.Destroyed -= OnInventoryDestroyed;
        PlayerUnitController.ConnectInventoriesRequest -= OnConnectInventoriesRequest;
        PlayerUnitController.DisconnectInventoriesRequest -= OnDisconnectInventoriesRequest;
        _selectionValueVariable.Modified -= OnSelectionModified;
    }

    private void OnInventoryInitialized(Inventory inventory)
    {
        var inventoryView = Instantiate(_inventoryViewPrefab, transform);
        inventoryView.Init(inventory, _camera, inventory.ViewOffset);
        _inventoryViews.Add(inventoryView);
    }

    private void OnInventoryDestroyed(Inventory inventory)
    {
        var inventoryView = _inventoryViews.Find(v => v.Inventory == inventory);
        if (inventoryView != null)
        {
            _inventoryViews.Remove(inventoryView);
            Destroy(inventoryView.gameObject);
        }
    }

    private void OnConnectInventoriesRequest(Inventory inventory1, Inventory inventory2)
    {
        if (GetConnection(inventory1, inventory2) == null)
        {
            inventory1.ConnectedInventories.Add(inventory2);
            inventory2.ConnectedInventories.Add(inventory1);
            _inventoryConnections.Add(new InventoryConnection(inventory1, inventory2));
        }

        RefreshInventoryViews();
    }

    private void OnDisconnectInventoriesRequest(Inventory inventory1, Inventory inventory2)
    {
        var connection = GetConnection(inventory1, inventory2);
        if (connection != null)
        {
            inventory1.ConnectedInventories.Remove(inventory2);
            inventory2.ConnectedInventories.Remove(inventory1);
            _inventoryConnections.Remove(connection);
        }

        RefreshInventoryViews();
    }

    private InventoryConnection GetConnection(Inventory inventory1, Inventory inventory2)
    {
        foreach (var _connection in _inventoryConnections)
        {
            if (_connection.Inventories.Contains(inventory1) && _connection.Inventories.Contains(inventory2))
            {
                return _connection;
            }
        }

        return null;
    }

    private void OnSelectionModified((List<Selectable> oldValue, List<Selectable> newValue) selection)
    {
        //RefreshInventoryViews();
    }

    private void RefreshInventoryViews()
    {
        //List<Inventory> inventoriesToShow = new List<Inventory>();

        //foreach (var inventoryView in _inventoryViews)
        //{
        //    if (inventoryView.Inventory.Owner.IsSelected)
        //    {
        //        inventoriesToShow.AddExclusive(inventoryView.Inventory);
        //    }
        //}

        //foreach (var connection in _inventoryConnections)
        //{
        //    bool anyInventorySelected = connection.Inventories.Any(i => i.Owner.IsSelected);

        //    if (anyInventorySelected)
        //    {
        //        inventoriesToShow.AddRange(connection.Inventories);
        //    }
        //}

        //_inventoryViews.Sort((v1, v2) => v1.Inventory.SortPriority.CompareTo(v2.Inventory.SortPriority));

        //foreach (var inventoryView in _inventoryViews)
        //{
        //    if (inventoriesToShow.Contains(inventoryView.Inventory))
        //    {
        //        inventoryView.Show();
        //    }
        //    else
        //    {
        //        inventoryView.Hide();
        //    }
        //}
    }

    [SerializeField]
    private void OnInteractionPopupRequested(PlayerUnitController unit, Targetable targetable)
    {
        if (targetable == null)
        {
            return;
        }

        InteractionPopup popup = Instantiate(targetable.InteractionPopupTemplate, transform);
        popup.Init(unit, targetable);
    }
}