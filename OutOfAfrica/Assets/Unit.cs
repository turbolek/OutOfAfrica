using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public event Action HealthChanged;

    [field: SerializeField] public float HP { get; private set; }
    [field: SerializeField] public float Strength;
    [field: SerializeField] public float AttackCooldown;

    public float CurrentHP
    {
        get { return _currentHP; }
        private set
        {
            _currentHP = value; HealthChanged?.Invoke();
        }
    }

    public bool IsDead { get; private set; }
    public bool IsFighting => _currentTarget != null && !_currentTarget.IsDead;
    public bool CanAttack => !IsDead && Time.time - _lastAttackTime >= AttackCooldown;

    private Unit _currentTarget;
    private float _lastAttackTime;
    private float _currentHP;

    private void Awake()
    {
        CurrentHP = HP;
        IsDead = false;
    }

    private void Update()
    {
        if (_currentTarget != null)
        {
            if (CanAttack)
            {
                Attack();
            }
        }
    }

    public void SetTarget(Unit unit)
    {
        if (unit != _currentTarget)
        {
            _lastAttackTime = Time.time;
        }
        _currentTarget = unit;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;
        SetTarget(null);
    }

    private void Attack()
    {
        if (_currentTarget == null || _currentTarget.IsDead)
        {
            return;
        }

        _currentTarget.TakeDamage(Strength);
        _lastAttackTime = Time.time;
    }
}