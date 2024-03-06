using System;
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
    private NavMeshAgent _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;
    private Targetable _currentTarget;
    private bool _wasMoving;
    private Vector3 _currentTargetPosition;
    private Command _currentCommand;

    private float _lastCommandTime;
    private NavMeshSurface _navMeshSurface; //TODO send navmesh update request instead of directly referring

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
            if (Time.time >= _lastCommandTime + _commandCooldown && _currentCommand.Validate())
            {
                _currentCommand.Perform();
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

    private Command GetCommand()
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
}