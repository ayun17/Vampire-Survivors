using System;
using System.Collections.Generic;

public sealed class StateMachine<T>
{
    private T _stateMachineController;
    public State<T> CurrentState { get; private set; }

    private Dictionary<Type, State<T>> stateList = new Dictionary<Type, State<T>>();

    public StateMachine(T stateMachineController, State<T> initState)
    {
        if (stateMachineController != null && initState != null)
        {
            _stateMachineController = stateMachineController;

            AddStateList(initState);
            CurrentState = initState;
            CurrentState.Enter();
        }
    }

    public void AddStateList(State<T> addState)
    {
        addState.SetMachineWithController(this, _stateMachineController);
        stateList[addState.GetType()] = addState;
    }

    public void DoUpdate()
    {
        if (CurrentState != null)
            CurrentState.OnUpdate();
    }

    public S ChangeState<S>() where S : State<T>
    {
        var newType = typeof(S);

        if (CurrentState.GetType() == newType)
            return CurrentState as S;

        if (CurrentState != null)
            CurrentState.Exit();

        CurrentState = stateList[newType];
        CurrentState.Enter();

        return CurrentState as S;
    }
}
