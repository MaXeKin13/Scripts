using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Transform plane;
    private void Update()
    {
        if (plane != null)
        {
            Vector3 manualPos = PlaneRaycast.RaycastToPlane(plane);
            transform.position = manualPos;
            return;
        }
        Vector3 pos = PlaneRaycast.RaycastToPlane(transform.position, Camera.main.transform.forward);

        transform.position = pos;
    }
}
