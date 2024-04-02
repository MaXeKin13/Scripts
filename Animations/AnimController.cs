using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AnimController : MonoBehaviour
{
    public bool AnimEvent = false;
    public UnityEvent[] animationEvent;
    public void AnimationEvent(int num)
    {
        animationEvent[num]?.Invoke();
    }


}
