using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyHit : State<NormalEnemy>
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log(" Àû ¸ÂÃã ");
    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
