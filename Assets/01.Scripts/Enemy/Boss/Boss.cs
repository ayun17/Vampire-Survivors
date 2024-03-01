using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour, IDamageable
{
    public UnityEvent<Vector2> isFaceDirection = null;
    public UnityEvent<float> onSpeedChanged = null;
    public UnityEvent onAttack = null;
    public UnityEvent onHit = null;
    public UnityEvent onDead = null;

    public EnemyDataSO enemyData;
    public GameObject bulletPrefab;

    public float speed;
    private int _damage = 1;
    private float _collTime = 0.5f;
    private float _lastAttackTime;

    private int currentHp;
    public int CurrentHp => currentHp;

    public bool IsAlive { get; private set; }

    public Vector2 moveDir = Vector2.zero;
    public LayerMask _player;
    public Transform target;
    public Rigidbody2D rigid;
    public PlayerHealth playerHealth;

    private StateMachine<Boss> _stateMachine;

    #region 넉백
    private bool _isKnockBack = false;
    private float _knockbackTime;
    private float _currentKnockbackTime;
    private Vector2 _knockbackDirection;
    #endregion

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Start()
    {
        //_stateMachine = new StateMachine<Boss>(this, new BulletEnemyMovement());
        //_stateMachine.AddStateList(new BulletEnemyHit());

        IsAlive = true;
        currentHp = enemyData.maxHP;
        speed = enemyData.maxSpeed;
        target = playerHealth.transform;
    }

    private void FixedUpdate()
    {
        if (IsAlive)
        {
            PlayerCheck();
            if (_isKnockBack)
                CalculateKnockback(); // 넉백중일때는 넉백상태 계산
        }
    }

    private void Update()
    {
        if (IsAlive)
        {
            _stateMachine.DoUpdate();

            rigid.velocity = moveDir * speed;

            onSpeedChanged?.Invoke(speed);
        }
    }

    public void OnDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            IsAlive = false;
            onDead?.Invoke();
            moveDir = Vector2.zero;
            Destroy(gameObject, 1f);
        }
        else
        {
            onHit?.Invoke();
            //_stateMachine.ChangeState<BulletEnemyHit>();
        }

    }

    private void CalculateKnockback()
    {
        _currentKnockbackTime += Time.fixedDeltaTime;
        float percent = _currentKnockbackTime / _knockbackTime;

        Vector2 knockbackDir = Vector2.Lerp(_knockbackDirection, moveDir * speed, percent);
        moveDir = knockbackDir.normalized; // 방향
        speed = knockbackDir.magnitude; // 길이

        if (percent >= 1)
        {
            _currentKnockbackTime = 0;
            _isKnockBack = false;
            StopImmediately();
        }
    }

    private void StopImmediately()
    {
        moveDir = Vector2.zero;
        speed = 0;

        //_stateMachine.ChangeState<BulletEnemyMovement>();
    }

    public void KnockBack(Vector2 direction, float time) // 넉백
    {
        if (direction != null)
        {
            _isKnockBack = true;
            _knockbackDirection = direction;
            _knockbackTime = time;
            _currentKnockbackTime = 0;
        }
    }

    private void PlayerCheck()
    {
        if (_lastAttackTime + _collTime > Time.time) return;

        Vector2 dir = target.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, enemyData.attackDistance, _player);

        if (hit.collider != null)
        {
            playerHealth.OnDamage(_damage); // 플레이어한태 데미지
            _lastAttackTime = Time.time;
        }
    }
}
