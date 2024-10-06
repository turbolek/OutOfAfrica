using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentWrapper : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private FloatVariable _timescale;


    private float _rawSpeed;

    private void OnEnable()
    {
        _timescale.Modified += OnTimescaleModified;
    }

    private void OnTimescaleModified((float oldValue, float newValue) values)
    {
        _navMeshAgent.speed = _rawSpeed * _timescale.Value;
    }


}
