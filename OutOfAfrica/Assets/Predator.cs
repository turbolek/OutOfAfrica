using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Predator : SerializedMonoBehaviour
{
    [SerializeField] private PlayerUnitControllerRange _reactionRange;
    [SerializeField] private PlayerUnitControllerRange _chaseRange;

    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;

    private PlayerUnitController _currentTarget;

    private void Update()
    {
        UpdateTarget();
        if (_currentTarget != null)
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.destination = _currentTarget.transform.position;
        }

        _navMeshAgent.enabled = IsMoving();
        _navMeshObstacle.enabled = !_navMeshAgent.enabled;

        if (!IsMoving())
        {
            _currentTarget = null;
        }
    }

    private void UpdateTarget()
    {
        _reactionRange.UpdateHits();
        _chaseRange.UpdateHits();

        if (_currentTarget == null || !_chaseRange.TargetsInRange.Contains(_currentTarget))
        {
            _currentTarget = GetNearestTarget(_reactionRange.TargetsInRange);
        }
    }

    private PlayerUnitController GetNearestTarget(List<PlayerUnitController> targets)
    {
        float minDistance = Mathf.Infinity;
        PlayerUnitController nearestTarget = null;

        foreach (var target in targets)
        {
            var targetDistance = Vector2.Distance(target.transform.position, transform.position);
            if (targetDistance < minDistance)
            {
                minDistance = targetDistance;
                nearestTarget = target;
            }
        }

        return nearestTarget;
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
}