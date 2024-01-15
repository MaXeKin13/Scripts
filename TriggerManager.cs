using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerManager : MonoBehaviour
{
    public string tag;
    public UnityEvent enterTrigger;
    public UnityEvent delayTrigger;

    public float delay;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(tag))
        {
            enterTrigger?.Invoke();
            StartCoroutine(delayInvoke());
        }
    }

    public void startDelay() => StartCoroutine(delayInvoke());

    private IEnumerator delayInvoke()
    {
        yield return new WaitForSeconds(delay);
        delayTrigger?.Invoke();
    }
}
