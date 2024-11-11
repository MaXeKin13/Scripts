using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerManager : MonoBehaviour
{
    public string triggerTag;
    public UnityEvent enterTrigger;
    public UnityEvent exitTrigger;
    public UnityEvent delayTrigger;

    public float delay;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag( ))
        {
            enterTrigger?.Invoke();
            StartCoroutine(delayInvoke());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.CompareTag(triggerTag))
        {
            exitTrigger?.Invoke();
        }
    }

    public void startDelay() => StartCoroutine(delayInvoke());

    private IEnumerator delayInvoke()
    {
        yield return new WaitForSeconds(delay);
        delayTrigger?.Invoke();
    }
}
