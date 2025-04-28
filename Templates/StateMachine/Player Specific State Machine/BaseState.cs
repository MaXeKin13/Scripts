using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public interface IState
{
    void EnterState(Player player);
    void ExitState(Player player);
    void UpdateState(Player player);
}*/
public abstract class BaseState
{

    protected Player player;

    //initialize Player (called in Player script)
    //so that we can access Player's methods and variables
    public virtual void Initialize(Player player)
    {
        this.player = player;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
}
