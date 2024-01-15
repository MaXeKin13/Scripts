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
        //
        DrawDefaultInspector();
        //Set current position to animation position
        if (GUILayout.Button("Set Point"))
        {
            _lineMovement.PathPoints[_lineMovement.currentPos].position = _lineMovement.transform.position;
            _lineMovement.PathPoints[_lineMovement.currentPos].rotation = _lineMovement.transform.eulerAngles;
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
    }
    private void OnSceneGUI()
    {
        MaxLineBezierMovement pathMovement = (MaxLineBezierMovement)target;

        if (pathMovement.PathPoints == null)
            return;

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
        }

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
            Handles.DrawLine(point.position, point.position + forwardVector *4);
        }
    }
    
    private void OnEnable() {
        if (!_lineMovement) {
            //target is object
            _lineMovement = target as MaxLineBezierMovement;
            
        }
    }
}
