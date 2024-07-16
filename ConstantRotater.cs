using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotater : MonoBehaviour
{
    public enum Axis
    {
        x,
        y, 
        z
    }
    public Axis axis;

    private Rigidbody rb;
    public Vector3 m_EulerAngleVelocity = new Vector3(0, 0, 0);
    public float rotateSpeed = 1f;

    private bool isRb;
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        
        isRb = (rb != null)? true: false;


        switch (axis) 
        {
            case (Axis.x):
                m_EulerAngleVelocity.x = 100;
                break;
            case (Axis.y):
                m_EulerAngleVelocity.y = 100;
                break;
            case (Axis.z):
                m_EulerAngleVelocity.z = 100;
                break;
                
        }
        Debug.Log(m_EulerAngleVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRb)
            RigidbodyRotation();
        else
            TransformRotation();
    }

    private void RigidbodyRotation()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * rotateSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
    private void TransformRotation()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * rotateSpeed * Time.fixedDeltaTime);
        transform.rotation = transform.rotation * deltaRotation;
    }
}
