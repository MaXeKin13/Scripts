using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AnimController : MonoBehaviour
{
    public bool AnimEvent = false;
    public UnityEvent animationEvent;
    public void AnimationEvent()
    {
        animationEvent?.Invoke();
    }

    private void Update()
    {
        if(AnimEvent)
            AnimationEvent();
    }
}
