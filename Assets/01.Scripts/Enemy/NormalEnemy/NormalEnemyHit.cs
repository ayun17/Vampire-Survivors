using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyHit : State<NormalEnemy>
{
    private float _knockBackForce = 10f;

    public override void Enter()
    {
        base.Enter();
        _stateMachineController.KnockBack(-_stateMachineController.moveDir * _knockBackForce, 0.3f);
    }
}
