using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{

    public Rigidbody followObject;


    public Vector3 offset;
    public float lerpAmount = 1f;



    private void Start()
    {

    }
    private void LateUpdate()
    {
        FollowPosition();

    }

    private void FollowPosition()
    {
        var velocity = followObject.velocity.normalized;

        // Calculate the target position
        Vector3 targetPosition = followObject.position - velocity;
        transform.position = Vector3.Lerp(transform.position, targetPosition + offset, Time.deltaTime * lerpAmount);
    }
}

