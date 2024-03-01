using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashShooterEnemyAttack : State<DashShooterEnemy>
{
    public override void Enter()
    {
        base.Enter();

        _stateMachineController.moveDir = Vector2.zero;
        _stateMachineController.speed = _stateMachineController.enemyData.maxSpeed;
        _stateMachineController.isFaceDirection?.Invoke(_stateMachineController.target.position);
        _stateMachineController.Attack();
    }
}
