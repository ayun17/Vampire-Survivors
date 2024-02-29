using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public UnityEvent onHit = null;
    public UnityEvent onDead = null;

    [SerializeField] private int maxHp;

    private int _currentHp;
    public int CurrentHp => _currentHp;

    private bool _isDead = false;
    public bool IsDead => _isDead;

    private void Start()
    {
        _currentHp = maxHp;
    }

    public void OnDamage(int damage)
    {
        _currentHp -= damage;

        if (_currentHp <= 0)
        {
            _isDead = true;
            onDead?.Invoke();
        }
        else
            onHit?.Invoke();
    }
}
