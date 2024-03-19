using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private void Update()
    {
        Vector3 pos = PlaneRaycast.RaycastToPlane(transform.position, Camera.main.transform.forward);

        transform.position = pos;
    }
}
