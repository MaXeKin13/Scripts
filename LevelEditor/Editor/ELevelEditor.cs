using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using log4net.Util;
using System.Runtime.CompilerServices;


[CustomEditor(typeof(LevelEditor))]
public class ELevelEditor : Editor
{
    private LevelEditor _levelEditor;

    private Vector3[,] grid;

    public override void OnInspectorGUI()
    {
        //
        DrawDefaultInspector();
        //Set current position to animation position
        if (GUILayout.Button("SpawnRow"))
        {
            _levelEditor.SetGrid();
            SceneView.RepaintAll();
        }
        if (GUILayout.Button("RemoveRow"))
        {
            _levelEditor.RemoveGrid();
            SceneView.RepaintAll();
        }
        if (GUILayout.Button("Flip"))
        {
            _levelEditor.direction *= -1;
        }




    }
    private void OnSceneGUI()
    {
        LevelEditor levelEditor = (LevelEditor)target;

        //Handles.DrawWireDisc
        // Handles.DrawWireCube(levelEditor.transform.position, levelEditor.cellSize);
        GetVisuals();
    }

    private void OnEnable()
    {
        if (!_levelEditor)
        {
            //target is object
            _levelEditor = target as LevelEditor;
        }

        GetCellSize();
    }
    private void GetVisuals()
    {
        LevelEditor levelEditor = (LevelEditor)target;

        GetCellSize();
        float boundsX = levelEditor.cellSize.x;
        float boundsY = levelEditor.cellSize.y;
        /*for (int y = 0; y < collumnLength; y++)
        {
            //spawn collumn first
            for (int x = 0; x < rowLength; x++)
            {
                grid[x, y] = Instantiate(block, new Vector3(transform.position.x, transform.position.y, 0f) + new Vector3(x * boundsX, y * boundsY, transform.position.z),
                Quaternion.identity, transform);
            }
        }*/


        Quaternion blockRotation = levelEditor.transform.rotation;
        Handles.matrix = Matrix4x4.TRS(levelEditor.transform.position, levelEditor.transform.rotation, Vector3.one);

        for (int y = 0; y < levelEditor.collumnLength; y++)
        {
            for (int x = 0; x < levelEditor.rowLength; x++)
            {
                Vector3 pos = new Vector3(x * boundsX * levelEditor.direction, y * boundsY, 0f);
                grid[x, y] = pos;
                Handles.DrawWireCube(pos, levelEditor.cellSize);
            }
        }

        Handles.matrix = Matrix4x4.identity; // Reset the Handles.matrix
    }
    private Vector3 GetCellSize()
    {
        LevelEditor levelEditor = (LevelEditor)target;
        grid = new Vector3[levelEditor.rowLength, levelEditor.collumnLength];
        levelEditor.SetCellSize();


        return levelEditor.cellSize;
    }
}
