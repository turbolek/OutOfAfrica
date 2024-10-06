using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
[RequireComponent(typeof(Selectable), typeof(Inventory))]
public class PlayerUnitController : MonoBehaviour
{
    public static Action<Inventory, Inventory> ConnectInventoriesRequest;
    public static Action<Inventory, Inventory> DisconnectInventoriesRequest;

    [field: SerializeField] public float BaseRadius { get; private set; }
    [SerializeField] private float _movementSpeed = 2f;
    [field: SerializeField] public float CommandCooldown { get; private set; } = 1f;

    private NavMeshAgentWrapper _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;
    private Targetable _currentTarget;
    private bool _wasMoving;
    private Vector3 _currentTargetPosition;
    private Command _currentCommand;
    public Inventory Inventory { get; private set; }
    public Selectable Selectable { get; private set; }

    private float _lastCommandTime;
    private NavMeshSurface _navMeshSurface; //TODO send navmesh update request instead of directly referring

    private List<Targetable> _targetablesInTouch = new();
    private List<Inventory> _openedInventories = new();

    private ItemSlot _pickupSourceInventorySlot;
    private ItemSlot _pickupTargetInventorySlot;

    private void OnEnable()
    {
        Targetable.UnitEntered += OnEnteredTarget;
        Targetable.UnitExited += OnExitedTarget;
    }

    private void Start()
    {
        Inventory = GetComponent<Inventory>();
        Selectable = GetComponent<Selectable>();
        _navMeshAgent = GetComponent<NavMeshAgentWrapper>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _navMeshSurface = FindFirstObjectByType<NavMeshSurface>();
    }


    private void Update()
    {
        _navMeshAgent.enabled = IsMoving();
        _navMeshObstacle.enabled = !_navMeshAgent.enabled;

        if (_wasMoving && !IsMoving())
        {
            OnTargetReached();
        }

        _wasMoving = IsMoving();

        if (_currentCommand != null)
        {
            if (Time.time >= _lastCommandTime + _currentCommand.GetCooldown())
            {
                if (_currentCommand.Validate())
                {
                    _currentCommand.Perform();
                }
                else
                {
                    _currentCommand = null;
                    SetPickupSlots(null, null);
                }

                _lastCommandTime = Time.time;
            }
        }
    }

    private void OnDisable()
    {
        Targetable.UnitEntered -= OnEnteredTarget;
        Targetable.UnitExited -= OnExitedTarget;
    }

    public void SetTarget(Targetable targetable, Vector3 targetPosition)
    {
        CloseInventories();
        _currentTarget = targetable;

        _targetablesInTouch.ClearNulls();

        if (_targetablesInTouch.Contains(targetable))
        {
            OnTargetReached();
            return;
        }

        _currentCommand = null;
        SetPickupSlots(null, null);
        SetMovementTarget(targetPosition);
    }

    public void SetMovementTarget(Vector3 targetPosition)
    {
        _navMeshObstacle.enabled = false;

        _navMeshSurface.BuildNavMesh();

        _navMeshAgent.enabled = true;
        _navMeshAgent.destination = targetPosition;
    }

    private bool IsMoving()
    {
        if (!_navMeshAgent.enabled)
        {
            return false;
        }

        if (!_navMeshAgent.pathPending)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void OnTargetReached()
    {
        var targetName = _currentTarget != null ? _currentTarget.name : "";
        Debug.Log($"{name} reached target: {targetName} at position: {_currentTargetPosition}");
        _currentCommand = GetCommand();
        if (_currentTarget)
        {
            var shift = _currentTarget.transform.position - transform.position;
            shift.y = 0f;
            transform.LookAt(transform.position + shift);
            _currentTarget.Interact(this);
        }
    }

    private Command GetCommand() //TODO use command factroy
    {
        if (_currentTarget == null)
        {
            return null;
        }

        CloseInventories();
        List<Inventory> inventoriesInTouch = new();

        _targetablesInTouch.ClearNulls();
        foreach (var targetable in _targetablesInTouch)
        {
            var inventories = targetable.GetComponentsInChildren<Inventory>().ToList();
            inventories.AddExclusive(targetable.GetComponent<Inventory>());
            foreach (var inventory in inventories)
            {
                if (inventory != null)
                {
                    inventoriesInTouch.Add(inventory);
                }
            }
        }

        if (inventoriesInTouch.Count > 0)
        {
            var inventoriesToOpen = new List<Inventory>(inventoriesInTouch);
            inventoriesToOpen.Add(Inventory);

            foreach (var inventory in inventoriesToOpen)
            {
                ConnectInventoriesRequest?.Invoke(inventory, Inventory);
                _openedInventories.AddExclusive(inventory);
            }
        }

        if (_pickupSourceInventorySlot != null)
        {
            var currentCollectCommand = _currentCommand as CollectItemCommand;
            if (currentCollectCommand == null || currentCollectCommand.SourceSlot != _pickupSourceInventorySlot ||
                currentCollectCommand.TargetSlot != _pickupTargetInventorySlot)
            {
                return new CollectItemCommand(this, _pickupSourceInventorySlot, _pickupTargetInventorySlot);
            }
        }

        // var resourceStack = _currentTarget.GetComponent<ItemStack>();
        // if (resourceStack)
        // {
        //     return new CollectItemCommand(this, _currentTarget);
        // }
        //
        // var dropOffZone = _currentTarget.GetComponent<DropOffZone>();
        // if (dropOffZone)
        // {
        //     return new DropOffResourcesCommand(this, _currentTarget);
        // }

        return null;
    }

    private void OnEnteredTarget(Targetable targetable, PlayerUnitController unit)
    {
        if (unit == this && _navMeshAgent.enabled)
        {
            _targetablesInTouch.AddExclusive(_currentTarget);
            _navMeshAgent.velocity = Vector3.zero;
            _navMeshAgent.destination = transform.position;
        }

        var predator = targetable.GetComponent<Predator>();
        if (predator)
        {

        }
    }


    private void OnExitedTarget(Targetable targetable, PlayerUnitController unit)
    {
        if (unit == this)
        {
            _targetablesInTouch.Remove(targetable);
            _currentCommand = GetCommand();
        }
    }

    private void EnterCombat(Predator predator)
    {

    }

    public bool CanPickupItem(Item item)
    {
        return Inventory.CanFitItem(item);
    }

    private void CloseInventories()
    {
        foreach (var inventory in _openedInventories)
        {
            DisconnectInventoriesRequest?.Invoke(inventory, Inventory);
        }
    }

    public void SetPickupSlots(ItemSlot sourceSlot, ItemSlot targetSlot)
    {
        _pickupSourceInventorySlot = sourceSlot;
        _pickupTargetInventorySlot = targetSlot;

        _currentCommand = GetCommand();
    }

    public bool HasTool(ToolCategory toolCategory)
    {
        if (toolCategory == null)
        {
            return true;
        }

        foreach (var itemSlot in Inventory.ItemSlots)
        {
            if (itemSlot.Item != null && itemSlot.Item.Data.ToolCategory == toolCategory)
            {
                return true;
            }
        }

        return false;
    }
}