using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public interface IState
{
    void EnterState(Player player);
    void ExitState(Player player);
    void UpdateState(Player player);
}*/
public abstract class BaseState<T>
{
    //whatever OWNS the state
    protected T owner;

    public virtual void Initialize(T owner)
    {
        this.owner = owner;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
}
