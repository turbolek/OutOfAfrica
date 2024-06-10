using System;
using UnityEngine;

[Serializable]
public class Range : MonoBehaviour
{
    private System.Action _onTargetEnteredCallback;
    private System.Action _onTargetExitedCallback;

    private Type _targetType;
    private bool _initialized;

    public void Init(Type targetType, Action onTargetEnteredCallback, Action onTargetExitedCallback)
    {
        _targetType = targetType;
        _onTargetEnteredCallback = onTargetEnteredCallback;
        _onTargetExitedCallback = onTargetExitedCallback;
        _initialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_initialized)
        {
            return;
        }

        var target = other.GetComponent(_targetType);
        if (target != null)
        {
            _onTargetEnteredCallback?.Invoke();
            Debug.Log($"Target: {target.name} entered range: {name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_initialized)
        {
            return;
        }

        var target = other.GetComponent(_targetType);
        if (target != null)
        {
            _onTargetExitedCallback?.Invoke();
            Debug.Log($"Target: {target.name} exited range: {name}");
        }
    }
}