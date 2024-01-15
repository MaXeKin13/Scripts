using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public float zDistance;
    private Vector2 mousePos;
    private Vector3 truePoint;


    private void Update()
    {
        GetMousePos();
    }

    private void GetMousePos()
    {
        //get input
        mousePos = Input.mousePosition;
        //translate to world position
        truePoint = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zDistance));
            
        MoveCursor();
    }

    private void MoveCursor()
    {
        transform.position = truePoint;
    }
}
