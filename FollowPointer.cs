using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPointer : MonoBehaviour
{
    public float zOffset;

    private Vector3 worldMousePosition;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        // Create a plane that matches the object's plane
        Plane plane = new Plane(Vector3.up, transform.position);

        // Cast a ray from the mouse position
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float hitDistance;

        // Check if the ray intersects the plane
        if (plane.Raycast(ray, out hitDistance))
        {
            // Get the intersection point
            Vector3 hitPoint = ray.GetPoint(hitDistance);

            // Set the object's position
            transform.position = new Vector3(hitPoint.x, transform.position.y, hitPoint.z + zOffset);
        }
    }
}