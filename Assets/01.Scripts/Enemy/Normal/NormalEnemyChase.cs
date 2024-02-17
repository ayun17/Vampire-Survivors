using UnityEngine;

public class NormalEnemyChase : State<NormalEnemy>
{
    public override void Enter()
    {
        base.Enter();
        _stateMachineController.speed = _stateMachineController.enemyData.maxSpeed;
        Debug.Log("�i�ư���");
    }

    public override void OnUpdate()
    {
        _stateMachineController.moveDir = (_stateMachineController.target.position - _stateMachineController.transform.position).normalized;

        _stateMachineController.isFaceDirection?.Invoke(_stateMachineController.target.position);

        // AttackState�� ����
        if (_stateMachineController.TargetInRange())
            _stateMachine.ChangeState<NormalEnemyAttack>();
    }
}
