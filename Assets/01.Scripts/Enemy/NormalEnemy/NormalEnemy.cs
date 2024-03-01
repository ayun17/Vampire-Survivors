using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NormalEnemy : MonoBehaviour, IDamageable
{
    public UnityEvent<Vector2> isFaceDirection = null;
    public UnityEvent<float> onSpeedChanged = null;
    public UnityEvent onAttackSuccess = null;
    public UnityEvent onHit = null;
    public UnityEvent onDead = null;

    public EnemyDataSO enemyData;

    public float speed;
    private int currentHp;
    public int CurrentHp => currentHp;
    public bool IsAlive { get; private set; }

    public Vector2 moveDir;
    public LayerMask _player;
    public Transform target;
    public Rigidbody2D rigid;
    public PlayerHealth playerHealth;

    private StateMachine<NormalEnemy> _stateMachine;

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
        _stateMachine = new StateMachine<NormalEnemy>(this, new NormalEnemyChase());
        _stateMachine.AddStateList(new NormalEnemyAttack());
        _stateMachine.AddStateList(new NormalEnemyHit());

        IsAlive = true;
        currentHp = enemyData.maxHP;
        speed = enemyData.maxSpeed;
        target = playerHealth.transform;
    }

    private void FixedUpdate()
    {
        if (_isKnockBack && IsAlive)
            CalculateKnockback(); // 넉백중일때는 넉백상태 계산
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
            _stateMachine.ChangeState<NormalEnemyHit>();
        }

    }

    public bool TargetInRange() // 타겟이 공격 범위 안에 있는지
    {
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance < enemyData.attackDistance)
            return true;
        return false;
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

        _stateMachine.ChangeState<NormalEnemyChase>();
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
}
