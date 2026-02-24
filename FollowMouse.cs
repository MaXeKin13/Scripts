using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public bool needClick = false;
    public Transform plane;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
        if (needClick)
        {
            return;
        }

        if (plane != null)
        {
            Vector3 manualPos = PlaneRaycast.RaycastToPlane(plane);
            transform.position = manualPos;
            return;
        }


        Vector3 pos = PlaneRaycast.RaycastToPlane(transform.position, Camera.main.transform.forward);

        transform.position = pos;
    }


    public void OnMouseDown()
    {
        if (needClick)
        {
            needClick = false;
        }
    }
}
