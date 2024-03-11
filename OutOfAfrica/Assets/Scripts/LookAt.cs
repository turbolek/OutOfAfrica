using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;

    private void Update()
    {
        if (_targetTransform != null)
        {
            transform.LookAt(_targetTransform);
        }
    }
}