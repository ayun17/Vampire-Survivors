using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SpawnEnemy : MonoBehaviour, IDamageable
{
    public UnityEvent<Vector2> isFaceDirection = null;
    public UnityEvent onSpawnEnemy = null;
    public UnityEvent onHit = null;
    public UnityEvent onDead = null;

    public EnemyDataSO enemyData;

    public float speed;
    private int _damage = 3;
    private float _collTime = 0.5f;
    private float _lastAttackTime;
    private int currentHp;
    public int CurrentHp => currentHp;

    public bool IsAlive { get; private set; }

    public Vector2 moveDir;
    public LayerMask _player;
    public Transform target;
    public Rigidbody2D rigid;
    public PlayerHealth playerHealth;

    private StateMachine<SpawnEnemy> _stateMachine;

    // Spawn 관련
    [SerializeField] private GameObject _childEnemy;
    private float _spawnCoolTime = 3f;
    private float _currentSpawnTime = 0;

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
        _stateMachine = new StateMachine<SpawnEnemy>(this, new SpawnEnemyIdle());
        _stateMachine.AddStateList(new SpawnEnemyHit());

        IsAlive = true;
        currentHp = enemyData.maxHP;
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
            Spawn();
        }
    }

    private void Spawn()
    {
        if (!_isKnockBack)
        {
            _currentSpawnTime += Time.deltaTime;
            if (_currentSpawnTime > _spawnCoolTime)
            {
                Instantiate(_childEnemy, transform.position, Quaternion.identity);
                onSpawnEnemy?.Invoke();
                // 생성 파티클 & 소리

                _currentSpawnTime = 0;
            }
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
            _stateMachine.ChangeState<SpawnEnemyHit>();
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

        _stateMachine.ChangeState<SpawnEnemyIdle>();
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
