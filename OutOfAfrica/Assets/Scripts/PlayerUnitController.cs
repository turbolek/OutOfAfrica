using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
[RequireComponent(typeof(Selectable))]
public class PlayerUnitController : MonoBehaviour
{
    [field: SerializeField] public float BaseRadius { get; private set; }
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _commandCooldown = 1f;
    [SerializeField] private int _inventoryCapacity = 10;

    public int RemainingInventoryCapacity
    {
        get
        {
            int cap = _inventoryCapacity;
            foreach (var resource in Inventory.Keys)
            {
                cap -= Inventory[resource];
            }

            return cap;
        }
    }

    private NavMeshAgent _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;
    private Targetable _currentTarget;
    private bool _wasMoving;
    private Vector3 _currentTargetPosition;
    private Command _currentCommand;

    private float _lastCommandTime;
    private NavMeshSurface _navMeshSurface; //TODO send navmesh update request instead of directly referring
    public Dictionary<ResourceType, int> Inventory { get; private set; } = new(); //TODO make a separate component

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
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
            if (Time.time >= _lastCommandTime + _commandCooldown)
            {
                if (_currentCommand.Validate())
                {
                    _currentCommand.Perform();
                }
                else
                {
                    _currentCommand = null;
                }

                _lastCommandTime = Time.time;
            }
        }
    }

    public void SetTarget(Targetable targetable)
    {
        _currentCommand = null;
        _currentTarget = targetable;

        if (targetable)
        {
            targetable.UnitEntered += OnEnteredTarget;
            targetable.UnitEntered += OnEnteredTarget;
        }
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
        }
    }

    private Command GetCommand() //TODO use command factroy
    {
        if (_currentTarget == null)
        {
            return null;
        }

        var resourceStack = _currentTarget.GetComponent<ResourceStack>();
        if (resourceStack)
        {
            return new CollectResourceCommand(this, _currentTarget);
        }

        var dropOffZone = _currentTarget.GetComponent<DropOffZone>();
        if (dropOffZone)
        {
            return new DropOffResourcesCommand(this, _currentTarget);
        }

        return null;
    }

    private void OnEnteredTarget(PlayerUnitController unit)
    {
        if (unit == this)
        {
            _navMeshAgent.velocity = Vector3.zero;
            _navMeshAgent.destination = transform.position;
            _currentTarget.UnitEntered -= OnEnteredTarget;
        }
    }

    private void OnExitedTarget(PlayerUnitController unit)
    {
    }

    public bool CanPickupResource(ResourceType resourceType)
    {
        return RemainingInventoryCapacity > 0;
    }

    public void PickupResource(ResourceType resourceType)
    {
        if (!Inventory.ContainsKey(resourceType))
        {
            Inventory.Add(resourceType, 0);
        }

        Inventory[resourceType] += 1;
    }

    public void DropResource(ResourceType resourceType)
    {
        if (Inventory.ContainsKey(resourceType))
        {
            Inventory[resourceType] = 0;
        }
    }

    public int GetResourceCount(ResourceType resourceType)
    {
        if (Inventory.ContainsKey(resourceType))
        {
            return Inventory[resourceType];
        }

        return 0;
    }
}