using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalState : BaseState
{
    public override void EnterState()
    {
        Debug.Log("Entering Normal State");
        //player.SetCanMove(true);
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Normal State");
    }

    public override void UpdateState()
    {
        Debug.Log("update Normal State");
    }


}
