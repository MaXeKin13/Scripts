using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickandDrag : MonoBehaviour
{
    public LayerMask layermask; // Updated to LayerMask type
    public float zDistance;
    public UnityEvent onClick;
    public UnityEvent onLetGo;
    

    private Transform _heldObject;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SendRaycast();
    }

    private void SendRaycast()
    {
        
        RaycastHit2D hit;
        Debug.Log("sendRaycast");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layermask);

        if (hit.collider != null)
        {
            _heldObject = hit.transform;
            StartCoroutine(MoveObject());
            onClick?.Invoke();
            //Get DragManager script from picked up object
            if(hit.transform.GetComponent<DragManager>())
                hit.transform.GetComponent<DragManager>().OnPickup();
        }
    }

    private IEnumerator MoveObject()
    {
        while (Input.GetMouseButton(0))
        {
            _heldObject.position =
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance));
            yield return null;
        }
        
        yield return new WaitForFixedUpdate();
        onLetGo?.Invoke();
    }

}
