using UnityEngine;
using UnityEngine.Events;

public class NormalEnemyAttack : State<NormalEnemy>
{

    private float _range = 0.5f; // 공격 사거리
    private float _damage = 1;

    private float _collTime = 1f;
    private float _lastAttackTime;

    public override void Enter()
    {
        base.Enter();
        _stateMachineController.speed = 0;
        _stateMachineController.moveDir = Vector3.zero;
        Debug.Log("공격 개시");
    }

    public override void OnUpdate()
    {
        Attack();

        if (!_stateMachineController.TargetInRange())
            _stateMachine.ChangeState<NormalEnemyChase>();
    }

    public void Attack()
    {
        if (_lastAttackTime + _collTime > Time.time) return;

        Vector2 dir = _stateMachineController.target.position - _stateMachineController.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(_stateMachineController.transform.position, dir.normalized, _range, _stateMachineController._player);

        if (hit.collider != null)
        {
            Debug.Log($"{_damage}만큼 공격");
            _stateMachineController.onAttackSuccess?.Invoke();
        }
        _lastAttackTime = Time.time;
    }
}
