using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    public Transform followObject;
    public float lerp;
    private void LateUpdate()
    {
        FollowRotate();
    }

    private void FollowRotate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, followObject.rotation, lerp* Time.deltaTime);
    }
}
