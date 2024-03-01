using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletEnemyMovement : State<BulletEnemy>
{
    RaycastHit2D hit;

    public override void Enter()
    {
        base.Enter();
        if (_stateMachineController.moveDir == Vector2.zero)
        {
            _stateMachineController.moveDir = RandomDirection();
            _stateMachineController.speed = _stateMachineController.enemyData.maxSpeed;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        _stateMachineController.isFaceDirection?.Invoke(_stateMachineController.moveDir);

        _stateMachineController.currentMoveTime += Time.deltaTime;
        if (_stateMachineController.currentMoveTime > _stateMachineController.moveChangeTime)
        {
            _stateMachineController.moveDir = RandomDirection();
            _stateMachineController.currentMoveTime = 0;
        }
    }

    private Vector2 RandomDirection()
    {
        Vector2 randVector = Random.insideUnitCircle.normalized;

        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(_stateMachineController.transform.position, randVector, 2f, 10);

            if (hit)
                randVector = Random.insideUnitCircle.normalized;
            else
                break;
        }

        return randVector;
    }
}
