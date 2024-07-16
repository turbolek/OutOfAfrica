using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public abstract class Range<T> : MonoBehaviour where T : Component
{
    private System.Action<T> _onTargetEnteredCallback;
    private System.Action<T> _onTargetExitedCallback;

    private bool _initialized;
    [SerializeField] private float _radius = 5f;

    [ReadOnly] [ShowInInspector] public List<T> TargetsInRange { get; private set; } = new();


    public void Init(Action<T> onTargetEnteredCallback, Action<T> onTargetExitedCallback)
    {
        _onTargetEnteredCallback = onTargetEnteredCallback;
        _onTargetExitedCallback = onTargetExitedCallback;
        _initialized = true;
    }

    public void UpdateHits()
    {
        var hits = Physics.OverlapSphere(transform.position, _radius);


        foreach (var hit in hits)
        {
            var target = hit.GetComponent<T>();
            if (target && !TargetsInRange.Contains(target))
            {
                TargetsInRange.Add(target);
                _onTargetEnteredCallback?.Invoke(target);
            }
        }

        for (int i = TargetsInRange.Count - 1; i >= 0; i--)
        {
            var target = TargetsInRange[i];
            if (hits.Length > 0 & hits.All(h => h.gameObject != target.gameObject))
            {
                TargetsInRange.Remove(target);
                _onTargetExitedCallback?.Invoke(target);
            }
        }
    }
}