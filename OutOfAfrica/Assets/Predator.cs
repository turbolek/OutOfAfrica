using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Predator : SerializedMonoBehaviour
{
    [SerializeField] private Range _reactionRange;
    [SerializeField] private Range _chaseRange;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private NavMeshObstacle _navMeshObstacle;

    private void Start()
    {
        _reactionRange.Init(typeof(PlayerUnitController), OnTargetEnteredReactionRange, OnTargetExitedReactionRange);
    }

    private void Update()
    {
        _navMeshAgent.enabled = IsMoving();
        _navMeshObstacle.enabled = !_navMeshAgent.enabled;
    }

    private void OnTargetEnteredReactionRange()
    {
    }

    private void OnTargetExitedReactionRange()
    {
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