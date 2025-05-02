using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : MonoBehaviour
{
    //CAN be an Interface
    //all classes must implement an "Execute" method
    public abstract void Execute(Player player);
}
