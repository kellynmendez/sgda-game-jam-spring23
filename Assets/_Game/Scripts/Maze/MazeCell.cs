using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    public bool leftWall;
    public bool downWall;

    public MazeCell()
    {
        // Every cell starts with a wall
        leftWall = true;
        downWall = true;
    }
}
