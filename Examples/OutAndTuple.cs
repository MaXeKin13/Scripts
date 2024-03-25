using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class OutAndTuples : MonoBehaviour
{
    void Start()
    {
        int i;
        
        TestBool(true, out i);
        Debug.Log(i);



        Tuple<bool, int> person = Tuple.Create(true, 0);

        TestTuple(person.Item1, person.Item2);
    }

    public bool TestBool(bool yes, out int i)
    {

        i = 1;
        return yes;
    }

    public (bool yes, int i) TestTuple(bool yes, int i)
    {
        yes = false;
        i = 1;
        return (yes, i);
    }
}
}
