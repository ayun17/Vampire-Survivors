using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyHit : State<BulletEnemy>
{
    private float _knockBackForce = 10f;

    public override void Enter()
    {
        base.Enter();

        Vector2 dir = -(_stateMachineController.target.position - _stateMachineController.transform.position).normalized;
        _stateMachineController.KnockBack(dir * _knockBackForce, 0.3f);
    }
}
