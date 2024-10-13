using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentWrapper : MonoBehaviour
{
    [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }
    [SerializeField] private BoolVariable _mapPauseVariable;

    private void OnEnable()
    {
        _mapPauseVariable.Modified += OnMapPauseChanged;
    }

    private void OnDisable()
    {
        _mapPauseVariable.Modified -= OnMapPauseChanged;
    }

    private void OnMapPauseChanged((bool newValue, bool oldValue) values)
    {
        if (NavMeshAgent.gameObject.activeInHierarchy && NavMeshAgent.enabled)
        {
            NavMeshAgent.isStopped = values.newValue;
        }
    }


}
