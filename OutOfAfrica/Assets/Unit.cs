using UnityEngine;

public class Unit : MonoBehaviour
{
    [field: SerializeField] public float HP;
    [field: SerializeField] public float Strength;
    [field: SerializeField] public float AttackCooldown;

    public bool IsDead { get; private set; }
    public bool IsFighting => _currentTarget != null && !_currentTarget.IsDead;
    public bool CanAttack => !IsDead && Time.time - _lastAttackTime >= AttackCooldown;

    private Unit _currentTarget;
    private float _lastAttackTime;

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
        _currentTarget = unit;
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        if (HP <= 0)
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