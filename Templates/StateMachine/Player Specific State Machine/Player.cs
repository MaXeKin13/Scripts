using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public StateMachine stateMachine { get; private set; }

    public BaseState currentState;
    public NormalState normalState = new NormalState();

    private void Awake()
    {
        //make new StateMachine specific for this object
        stateMachine = new StateMachine();
    }
    private void Start()
    {    
        normalState.Initialize(this);

        //set default state
        stateMachine.Initialize(normalState);
    }
    
}
