using UnityEngine;

public class NormalEnemyChase : State<NormalEnemy>
{
    public override void Enter()
    {
        base.Enter();
        _stateMachineController.speed = _stateMachineController.enemyData.maxSpeed;
        Debug.Log("쫒아가자");
    }

    public override void OnUpdate()
    {
        _stateMachineController.moveDir = (_stateMachineController.target.position - _stateMachineController.transform.position).normalized;

        _stateMachineController.isFaceDirection?.Invoke(_stateMachineController.target.position);

        // AttackState로 전이
        if (_stateMachineController.TargetInRange())
            _stateMachine.ChangeState<NormalEnemyAttack>();
    }
}
