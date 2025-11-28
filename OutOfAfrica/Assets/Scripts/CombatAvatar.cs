using System;
using UnityEngine;

public class CombatAvatar : MonoBehaviour
{
    [SerializeField] private Healthbar _healthBar;
    public event Action HealthChanged;
    public Unit Unit { get; private set; }

    private void OnDestroy()
    {
        Unit.HealthChanged -= OnHealthChanged;
        Unit = null;
    }

    public void Init(Unit unit)
    {
        Unit = unit;
        Unit.HealthChanged += OnHealthChanged;
        _healthBar.Init(this);
    }

    private void OnHealthChanged()
    {
        HealthChanged?.Invoke();
    }

}
