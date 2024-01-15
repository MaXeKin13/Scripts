using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickandDrag3D : MonoBehaviour
{
    public LayerMask layermask;
   // public float zDistance = 10f; // Adjust the distance from the camera to the dragged object
    public Vector3 panelOffset;
    public UnityEvent onClick;
    public UnityEvent onLetGo;
    public bool limitZ;
    [Space]
    //move to pickup pos
    
    
    private Transform _heldObject;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
        while (Input.GetMouseButton(0))
        {
            
            
            // Transform the mouse input from screen space to world space
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            //Create Plane with normal of Vector3.up, and at position _heldObject.Position
            //Makes a Plane parallel to the ground
            Plane plane = new Plane(Vector3.up, _heldObject.position);
            float distance;
            //Calculate intersection point of the ray (done above) with the plane and stores the distance from the ray's origin.
            plane.Raycast(ray, out distance);
            Vector3 newPosition = ray.GetPoint(distance);


            if (limitZ)
            {
                newPosition.z = _heldObject.position.z;
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
        onLetGo?.Invoke();
    }

}