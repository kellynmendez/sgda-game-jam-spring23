using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMaze : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator mazeGen = FindObjectOfType<MazeGenerator>();
        MazeCell [,] maze = mazeGen.GenerateMazeArray();
        mazeGen.PrintMaze(maze);
    }
}
