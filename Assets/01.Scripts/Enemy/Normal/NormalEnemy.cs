using UnityEngine;
using UnityEngine.Events;

public class NormalEnemy : MonoBehaviour, IDamageable
{
    public UnityEvent<Vector2> isFaceDirection = null;
    public UnityEvent<float> onSpeedChanged = null;
    public UnityEvent onAttackSuccess = null;

    public EnemyDataSO enemyData;

    public float speed;
    private int currentHp;
    public int CurrentHp => currentHp;
    public bool IsAlive { get; private set; }

    public LayerMask _player;
    public Vector2 moveDir;
    public Transform target;
    public Rigidbody2D rigid;

    private StateMachine<NormalEnemy> _stateMachine;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Start()
    {
        _stateMachine = new StateMachine<NormalEnemy>(this, new NormalEnemyChase());
        _stateMachine.AddStateList(new NormalEnemyAttack());
        _stateMachine.AddStateList(new NormalEnemyHit());

        IsAlive = true;
        currentHp = enemyData.maxHP;
        speed = enemyData.maxSpeed;
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
        Debug.Log($"나 적인데 {damage}만큼 피해 입어써");
        //_stateMachine.ChangeState<NormalEnemyHit>();
    }

    public bool TargetInRange() // 타겟이 공격 범위 안에 있는지
    {
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance < enemyData.attackDistance)
            return true;
        return false;
    }
}
