using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashShooterEnemy : MonoBehaviour, IDamageable
{
    public UnityEvent<Vector2> isFaceDirection = null;
    public UnityEvent<float> onSpeedChanged = null;
    public UnityEvent onAttack = null;
    public UnityEvent onHit = null;
    public UnityEvent onDead = null;

    [SerializeField] private GameObject bulletPrefab;
    public EnemyDataSO enemyData;

    public float speed;
    private int _damage = 1;
    private float _dashTime = 0.4f;
    private float _collTime = 0.5f;
    private float _lastAttackTime;

    private int currentHp;
    public int CurrentHp => currentHp;

    public bool IsAlive { get; private set; }
    public bool IsAttack = false;

    public Vector2 moveDir;
    public LayerMask _player;
    public Transform target;
    public Rigidbody2D rigid;
    public PlayerHealth playerHealth;

    private StateMachine<DashShooterEnemy> _stateMachine;

    // Attack 관련
    public float attackCoolTime = 3.5f;
    public float currentAttackTime = 0;

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
        _stateMachine = new StateMachine<DashShooterEnemy>(this, new DashShooterEnemyWait());
        _stateMachine.AddStateList(new DashShooterEnemyAttack());
        _stateMachine.AddStateList(new DashShooterEnemyHit());

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
            _stateMachine.ChangeState<DashShooterEnemyHit>();
        }
    }

    public void Attack()
    {
        StartCoroutine(DashRotine());
    }

    private IEnumerator DashRotine()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        moveDir = dir;

        float currentTimem = 0;
        while (true)
        {
            currentTimem += Time.deltaTime;
            if (currentTimem > _dashTime)
                break;

            yield return null;
        }

        speed = 0;
        moveDir = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShootRotine());
    }

    private IEnumerator ShootRotine()
    {
        isFaceDirection?.Invoke(target.position);

        int shootCnt = 3;
        Quaternion quaternion = Quaternion.FromToRotation(Vector2.right, target.position - transform.position);
        while (shootCnt > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, quaternion);
            Destroy(bullet, 1.5f);
            onAttack?.Invoke();

            yield return new WaitForSeconds(0.3f);
            shootCnt--;
        }

        _stateMachine.ChangeState<DashShooterEnemyWait>();
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

        _stateMachine.ChangeState<DashShooterEnemyWait>();
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
