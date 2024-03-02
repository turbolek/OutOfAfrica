using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
[RequireComponent(typeof(Selectable))]
public class PlayerUnitController : MonoBehaviour
{
    [field: SerializeField] public float BaseRadius { get; private set; }
    [SerializeField] private float _movementSpeed = 2f;
    private NavMeshAgent _navMeshAgent;
    private NavMeshObstacle _navMeshObstacle;
    private Targetable _currentTarget;
    private bool _wasMoving;
    private Vector3 _currentTargetPosition;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
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
    }

    public void SetTarget(Targetable targetable)
    {
        _currentTarget = targetable;
    }

    public void SetMovementTarget(Vector3 targetPosition)
    {
        _currentTargetPosition = targetPosition;
        _navMeshObstacle.enabled = false;
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
    }
}