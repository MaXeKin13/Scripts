using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{

    public BaseState<T> CurrentState { get; private set; }
    private T owner;


    public StateMachine(T owner)
    {
        this.owner = owner;
    }
   public void Initialize(BaseState<T> startingState)
    {
        CurrentState = startingState;
        CurrentState.Initialize(owner);
        CurrentState.EnterState();
    }

    public void SetState(BaseState<T> newState)
    {
        CurrentState?.ExitState();
        CurrentState = newState;
        CurrentState.Initialize(owner);
        CurrentState.EnterState();
    }

    public void Update()
    {
        CurrentState?.UpdateState();
    }

    
}
