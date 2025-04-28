using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public BaseState CurrentState { get; private set; }

    public void Initialize(BaseState startingState)
    {
        CurrentState = startingState;
        startingState.EnterState();
    }

    public void SetState(BaseState newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        newState.EnterState();
    }

    public void Update()
    {
        CurrentState?.UpdateState();
    }

    
}
