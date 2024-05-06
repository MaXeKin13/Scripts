using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public GameObject block;
    public int direction = 1;
    //grid constrained to certain area (have to set bounds), instead snap to certain position.
    //grid only useful if you need to know neighbors, etc;
    //use local position for everything + parent it

    public int rowLength;
    public int collumnLength;

    //size of each grid
    public Vector3 cellSize;


    //2d array for grid data (center positions)
    public GameObject[,] grid;
    //grid position can be different from grid value;


    public void SetGrid()
    {
        if (grid == null)
        {
            grid = new GameObject[rowLength, collumnLength];
        }
        //get size of block
        SetCellSize();

        //set bounds
        /*int boundsX = Mathf.RoundToInt(cellSize.x);
        int boundsY = Mathf.RoundToInt(cellSize.y);
        */
        float boundsX = (cellSize.x);
        float boundsY = (cellSize.y);

        for (int y = 0; y < collumnLength; y++)
        {
            //spawn collumn first
            for (int x = 0; x < rowLength; x++)
            {
                /* //get rotation of level editor and instantiate from there?
                 Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0f) + new Vector3(x * boundsX * direction, y * boundsY, transform.position.z);
                 //get rotation of object, apply it to an empty 
                 //inverse transform/transform
                 grid[x, y] = Instantiate(block, pos,
                     Quaternion.identity, transform);          */

                // Calculate local position relative to the transform's rotation
                Vector3 localPos = new Vector3(x * boundsX * direction, y * boundsY, 0f);
                // Transform local position to world position
                Vector3 pos = transform.TransformPoint(localPos);
                grid[x, y] = Instantiate(block, pos, transform.rotation, transform);
            }
        }
    }

    public void RemoveGrid()
    {
        foreach (GameObject go in grid)
        {
            DestroyImmediate(go);
        }
        grid = null;
    }

    public void SetCellSize()
    {
        cellSize = block.GetComponentInChildren<MeshRenderer>().bounds.size;
    }

    //modularizing attempt
    public Vector3 GetGridPos(int x, int y)
    {
        int boundsX = Mathf.RoundToInt(cellSize.x);
        int boundsY = Mathf.RoundToInt(cellSize.y);

        return new Vector3(x * boundsX * direction, y * boundsY, 0f);
    }
}
