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

    public enum Direction
    {
        up,
        down,
        forward,
        back
    }

    public Direction dir = Direction.up;

    public void SetGrid()
    {
        if (grid == null)
        {
            grid = new GameObject[rowLength, collumnLength];
        }
        //get size of block
        SetCellSize();


        for (int y = 0; y < collumnLength; y++)
        {
            //spawn collumn first
            for (int x = 0; x < rowLength; x++)
            {
                //ChangeWhere to Spawn depending on Axis

                // Calculate local position relative to the transform's rotation
                Vector3 localPos = GetPosition(dir, x, y);
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
    public Vector3 GetPosition(Direction dir, int x, int y)
    {
        switch (dir)
        {
            case Direction.up:
                return new Vector3(cellSize.x * x, y * cellSize.y, 0f);
            case Direction.down:
                return new Vector3(cellSize.x * x, cellSize.y * -y, 0f);
            case Direction.forward:
                return new Vector3(cellSize.x * x, 0, cellSize.z * y);
            case Direction.back:
                return new Vector3(cellSize.x * x, 0f, cellSize.z * -y);
            default:
                return new Vector3(cellSize.x * x, y * cellSize.y, 0f);

        }

    }
    
}
