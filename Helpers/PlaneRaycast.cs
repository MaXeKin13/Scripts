using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRaycast
{

    public static Vector3 RaycastToPlane(Vector3 planePosition, Vector3 planeNormal)
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a plane using the provided position and normal
        Plane plane = new Plane(planeNormal, planePosition);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            // Get the intersection point
            Vector3 intersectionPoint = ray.GetPoint(distance);
            return intersectionPoint;
        }

        // If no intersection, return Vector3.zero or some other default value
        return Vector3.zero;
    }

    //Raycast to specific object
    public static Vector3 RaycastToPlane(Transform plane)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        Plane rayPlane = new Plane(plane.up, plane.position);
        if (rayPlane.Raycast(ray, out distance))
        {
            Vector3 intersectionPoint = ray.GetPoint(distance);
            return intersectionPoint;
        }
        return Vector3.zero;

    }
    public static Vector3 RaycastToPlane(float distanceFromCamera)
    {

        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Calculate the position of the plane based on the distance from the camera along the ray
        Vector3 planePosition = ray.origin + ray.direction * distanceFromCamera;

        // Calculate the normal of the plane (which will be the negative of the ray's direction)
        Vector3 planeNormal = -ray.direction;
        // Create a plane using the provided position and normal
        Plane plane = new Plane(planeNormal, planePosition);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            // Get the intersection point
            Vector3 intersectionPoint = ray.GetPoint(distance);
            return intersectionPoint;
        }

        // If no intersection, return Vector3.zero or some other default value
        return Vector3.zero;
    }


}
