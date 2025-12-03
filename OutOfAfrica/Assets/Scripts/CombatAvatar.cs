using System;
using UnityEngine;

public class CombatAvatar : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Healthbar _healthBar;
    public event Action HealthChanged;
    public Unit Unit { get; private set; }

    private const string AttackTrigger = "Attack";
    private const string DieTrigger = "Die";

    private void OnDestroy()
    {
        Unit.HealthChanged -= OnHealthChanged;
        Unit.Died -= OnDied;
        Unit.Attacked -= OnAttacked;
        Unit = null;
        _healthBar.Clear();
    }

    public void Init(Unit unit)
    {
        Unit = unit;
        Unit.HealthChanged += OnHealthChanged;
        Unit.Died += OnDied;
        Unit.Attacked += OnAttacked;
        _healthBar.Init(this);
    }

    private void OnHealthChanged()
    {
        HealthChanged?.Invoke();
    }

    private void OnDied()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(DieTrigger);
        }

    }

    private void OnAttacked()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(AttackTrigger);
        }
    }

}
