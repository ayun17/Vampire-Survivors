using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float radius = 3.5f;

    private float _desireAngle;

    private bool _isFindTarget;

    private Vector2 _targetDir;
    private Vector2 _targetPos;
    private IDamageable _iDamageable;
    private Weapon _currentWeapon;

    private void Awake()
    {
        _currentWeapon = transform.Find("Knife").GetComponent<Weapon>();
    }

    private void Update()
    {
        if (!_currentWeapon.isAttacking)
            TargetCheck();
    }

    private void TargetCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        float shortestDistance = 4f;
        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<IDamageable>() != null)
            {
                if (Vector2.Distance(transform.position, collider.transform.position) < shortestDistance)
                {
                    _isFindTarget = true;

                    _targetPos = collider.transform.position;
                    _targetDir = collider.transform.position - transform.position;
                    _iDamageable = collider.GetComponent<IDamageable>();
                    shortestDistance = Vector2.Distance(transform.position, collider.transform.position);
                }
            }
        }

        if (_isFindTarget)
            Attack();
        _isFindTarget = false;
    }

    public void Attack() // 공격 할 적이 있을 때랑 연결
    {
        AimWeapon();
        _currentWeapon.UseWeapon(_targetPos);
        Debug.Log("칼로 공격");
    }

    public void GiveDamage()
    {
        _iDamageable.OnDamage(_currentWeapon.weaponDamage);
    }

    private void AimWeapon() // 공격 방향 바라보기
    {
        _desireAngle = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(_desireAngle, Vector3.forward);
    }
}
