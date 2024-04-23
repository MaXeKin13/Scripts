using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaxLineBezierMovement))]
public class EMaxLineMovement : Editor
{
    private MaxLineBezierMovement _lineMovement;


    //where buttons are in inspector
    public override void OnInspectorGUI()
    {
        MaxLineBezierMovement pathMovement = (MaxLineBezierMovement)target;
        //
        DrawDefaultInspector();
        //Set current position to animation position
        if (GUILayout.Button("Set Point"))
        {
            _lineMovement.PathPoints[_lineMovement.currentPos].position = _lineMovement.transform.position;
            _lineMovement.PathPoints[_lineMovement.currentPos].rotation = _lineMovement.transform.eulerAngles;
            _lineMovement.PathPoints[_lineMovement.currentPos].scale = _lineMovement.transform.localScale;
            SceneView.RepaintAll();
        }
        //create new point AFTER currentPosition
        if (GUILayout.Button("New Point"))
        {
            //create new point from currentPos
            MaxLineBezierMovement.PathPoint newPath = _lineMovement.PathPoints[_lineMovement.currentPos];
            //insert point at next position
            _lineMovement.PathPoints.Insert(_lineMovement.currentPos, newPath);
            SceneView.RepaintAll();
        }

        if (GUILayout.Button("Set All Straight"))
        {
            foreach (var pathPoint in _lineMovement.PathPoints)
            {
                pathPoint.controlPoint1 = pathPoint.position;
                pathPoint.controlPoint2 = pathPoint.position;
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Move To Position"))
        {
            _lineMovement.transform.position = _lineMovement.PathPoints[_lineMovement.currentPos].position;
            _lineMovement.transform.eulerAngles = _lineMovement.PathPoints[_lineMovement.currentPos].rotation;
            _lineMovement.transform.localScale = _lineMovement.PathPoints[_lineMovement.currentPos].scale;

        }


        //Handle Button Presses

        //Unity's current event in the Event System
        Event e = Event.current;
        Debug.Log(e.ToString());
        //check type of current event
        switch (e.type)
        {
            //if current event is a keydown press event
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.RightArrow))
                    {
                        pathMovement.currentPos++;
                    }
                    if (Event.current.keyCode == KeyCode.LeftArrow)
                    { pathMovement.currentPos--; }
                    break;
                }
        }
        //end 


    }

    private void OnSceneGUI()
    {
        //Debug.Log("SceneGUI");
        MaxLineBezierMovement pathMovement = (MaxLineBezierMovement)target;

        if (pathMovement.PathPoints == null)
            return;

        //Style for label
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.red;
        labelStyle.fontSize = 18;
        //

        //Draw Line between points
        for (var i = 0; i < pathMovement.PathPoints.Count - 1; i++)
        {
            var point = pathMovement.PathPoints[i];
            var nextPoint = pathMovement.PathPoints[i + 1];
            //draw lines between points
            Handles.DrawBezier(point.position, nextPoint.position,
                point.controlPoint1,
                nextPoint.controlPoint2,
                Color.green,
                Texture2D.normalTexture,
                1f);


            // Display point number
            Handles.Label(point.position, i.ToString(), labelStyle);
        }
        // Display number for the last point
        if (pathMovement.PathPoints.Count != 0)
            Handles.Label(pathMovement.PathPoints[pathMovement.PathPoints.Count - 1].position,
                (pathMovement.PathPoints.Count - 1).ToString(), labelStyle);

        //Draw Disc at point locations
        foreach (var point in pathMovement.PathPoints)
        {
            Handles.color = Color.cyan;
            Vector3 position = point.position;
            //Vector3 position = pathMovement.trans.transform.position;
            float handleSize = HandleUtility.GetHandleSize(position) * 0.2f;
            Handles.DrawWireDisc(position, Vector3.up, handleSize);

            //Rotation
            var rot = point.rotation;
            Quaternion rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
            Vector3 forwardVector = rotation * Vector3.forward;
            Handles.DrawLine(point.position, point.position + forwardVector * 4);
        }



    }

    private void OnEnable()
    {
        if (!_lineMovement)
        {
            //target is object
            _lineMovement = target as MaxLineBezierMovement;

        }
    }
}
