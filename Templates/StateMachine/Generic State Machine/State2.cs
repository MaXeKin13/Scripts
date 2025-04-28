using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State2 : BaseState<Player>
{
    public override void EnterState()
    {
        Debug.Log("Entering State 2");
    }
    public override void UpdateState()
    {
        Debug.Log("Updating State 2");
    }
    public override void ExitState()
    {
        Debug.Log("Exiting State 2");
    }
}
