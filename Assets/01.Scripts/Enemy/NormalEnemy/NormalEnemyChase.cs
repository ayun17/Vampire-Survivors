using UnityEngine;

public class NormalEnemyChase : State<NormalEnemy>
{
    public override void Enter()
    {
        base.Enter();
        _stateMachineController.speed = _stateMachineController.enemyData.maxSpeed;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        _stateMachineController.moveDir = (_stateMachineController.target.position - _stateMachineController.transform.position).normalized;

        _stateMachineController.isFaceDirection?.Invoke(_stateMachineController.target.position);

        // AttackState∑Œ ¿¸¿Ã
        if (_stateMachineController.TargetInRange())
            _stateMachine.ChangeState<NormalEnemyAttack>();
    }
}
