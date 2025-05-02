using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public StateMachine<Player> stateMachine { get; private set; }

    private void Awake()
    {
        // Create a new StateMachine for the Player
        stateMachine = new StateMachine<Player>(this);
    }

    private void Start()
    {
        // Initialize and set the default state in the state machine
        stateMachine.Initialize(new NormalState());
    }


    

}
