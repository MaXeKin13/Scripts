using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Plane = UnityEngine.Plane;
using Vector3 = UnityEngine.Vector3;

public class ClickandDrag3D : MonoBehaviour
{
    public LayerMask layermask;
    public float zDistance = 10f; // Adjust the distance from the camera to the dragged object
    public Vector3 panelOffset;
    public UnityEvent onClick;
    public UnityEvent onLetGo;
    public bool limitY;
    public bool limitZ;
    [Space]

    private Transform _heldObject;
    private Camera _mainCamera;
    private Vector3 _ogPos;
    public bool _canPickup;
    private void Start()
    {
        _mainCamera = Camera.main;
        _ogPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canPickup)
        {
            SendRaycast();
        }
    }

    private void SendRaycast()
    {
        RaycastHit hit;

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
        {
            _heldObject = hit.transform;
            StartCoroutine(MoveObject());
            onClick?.Invoke();

            // Get DragManager script from picked up object
            if (_heldObject.GetComponent<DragManager>())
            {
                _heldObject.GetComponent<DragManager>().OnPickup();
            }
        }
    }

    private IEnumerator MoveObject()
    {
        var child = transform.GetChild(0);
        if(child.GetComponent<Animator>()!=null)
            child.GetComponent<Animator>().enabled = false;
        child.rotation = Quaternion.Euler(0, -80f, 0);
        while (Input.GetMouseButton(0))
        {
            
            
            // Transform the mouse input from screen space to world space
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            //Create Plane with normal of Vector3.up, and at position _heldObject.Position
            //Makes a Plane parallel to the ground
            Plane plane = new Plane(Vector3.forward, _heldObject.position);
            float distance;
            //Calculate intersection point of the ray (done above) with the plane and stores the distance from the ray's origin.
            plane.Raycast(ray, out distance);
            Vector3 newPosition = ray.GetPoint(distance);


            //Limits
            if (limitZ)
            {
                newPosition.z = _heldObject.position.z;
            }
            if (limitY)
            {
                newPosition.y = +_heldObject.position.y;
            }

            // Set the new position relative to the camera's rotation
            _heldObject.position = newPosition;

            yield return null;
            
            
            //old
            /*Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 newPosition = ray.GetPoint(zDistance);
            _heldObject.position = newPosition;
            yield return null;*/
        }

        yield return new WaitForFixedUpdate();
        if (_heldObject.GetComponent<CashierItem>().inScanArea)
        {
            Debug.Log("drop");
            onLetGo?.Invoke();
        }
        else
        {
            GetComponentInChildren<ItemVisual>().KillTween();
            Debug.Log("send to og Pos");
            _heldObject.position = _ogPos;
            _heldObject.GetChild(0).transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void SetCanDrag() => _canPickup = !_canPickup;
}
