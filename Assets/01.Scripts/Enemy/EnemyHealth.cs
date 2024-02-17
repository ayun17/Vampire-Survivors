using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemyHealth : MonoBehaviour
{
    public UnityEvent OnGetHit;
    public UnityEvent OnDie;

    [SerializeField] private EnemyDataSO enemyData;

    public bool isDead = false;
    private bool _isKnockBack = false;

    private float _currentHealth;
    private float _knockbackTime;
    private float _currentKnockbackTime;

    private Vector2 _knockbackDirection;

    #region ³Ë¹é°ü·Ã ¼Ó¼º


    #endregion

    private void Start()
    {
        _currentHealth = enemyData.maxHP;
    }

    public void GetHit(int damage)
    {
        if (isDead) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
            DeadProcess();

        OnGetHit?.Invoke();
    }

    private void DeadProcess()
    {
        isDead = true;
        OnDie?.Invoke();
        Debug.Log("À¸¾Ç");
    }

    private void CalculateKnockback(Vector3 moveDirection, float currentSpeed)
    {
        _currentKnockbackTime += Time.fixedDeltaTime;
        float percent = _currentKnockbackTime / _knockbackTime;

        // _knockbackDirection ¾ê´Â ³Ë¹éÀÇ Èû°ú ¹æÇâÀÌ ¸ðµÎ Á¸Àç
        Vector2 moveDir = Vector2.Lerp(_knockbackDirection, moveDirection * currentSpeed, percent);
        moveDirection = moveDir.normalized; // ¹æÇâ
        currentSpeed = moveDir.magnitude; // ±æÀÌ

        if (percent >= 1)
        {
            _currentKnockbackTime = 0;
            _isKnockBack = false;

            moveDirection = Vector2.zero;
            currentSpeed = 0;
        }
    }

    public void KnockBack(Vector2 direction, float time)
    {
        if (!isDead)
        {
            _isKnockBack = true;
            _knockbackDirection = direction;
            _knockbackTime = time;
            _currentKnockbackTime = 0;
        }
    }
}
