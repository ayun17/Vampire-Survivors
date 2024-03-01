using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyIdle : State<SpawnEnemy>
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        _stateMachineController.isFaceDirection?.Invoke(_stateMachineController.target.position);
    }
}
