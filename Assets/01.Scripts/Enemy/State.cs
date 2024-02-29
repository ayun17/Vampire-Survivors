using UnityEngine;

public abstract class State<T>
{
    protected StateMachine<T> _stateMachine;
    protected T _stateMachineController;

    public virtual void OnAwake() { }
    public virtual void Enter() { }
    public virtual void OnUpdate() { }
    public virtual void Exit() { }

    public void SetMachineWithController(StateMachine<T> stateMachine, T stateMachineController)
    {
        _stateMachine = stateMachine;
        _stateMachineController = stateMachineController;
        OnAwake();
    }
}
