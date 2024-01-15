using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragManager : MonoBehaviour
{
    public UnityEvent onPickupEvent;

    public void OnPickup()
    {
        onPickupEvent?.Invoke();
    }
}
