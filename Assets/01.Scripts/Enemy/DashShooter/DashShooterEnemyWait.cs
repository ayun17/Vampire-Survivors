using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashShooterEnemyWait : State<DashShooterEnemy>
{
    public override void Enter()
    {
        base.Enter();

        _stateMachineController.IsAttack = false;

        _stateMachineController.speed = 0;
        _stateMachineController.moveDir = Vector2.zero;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        _stateMachineController.currentAttackTime += Time.deltaTime;
        if (_stateMachineController.currentAttackTime > _stateMachineController.attackCoolTime)
        {
            _stateMachineController.IsAttack = true;

            _stateMachineController.currentAttackTime = 0;
            _stateMachine.ChangeState<DashShooterEnemyAttack>();
        }
    }
}
