using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] int numRows = 10;
    [SerializeField] int numCols = 10;
    [SerializeField] int wallLength = 15;
    [SerializeField] int wallDepth = 1;
    [SerializeField] GameObject wallPrefab;

    /**
     * Generates a maze stored in a 2D array of cells
     */
    public MazeCell[,] GenerateMazeArray()
    {
        int numCells = numRows * numCols;

        // Instantiating the maze array with cells
        MazeCell[,] maze = new MazeCell[numRows, numCols];
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                maze[i, j] = new MazeCell();
            }
        }

        // Setting the start and end
        maze[0, 0].leftWall = false;
        maze[numRows - 1, numCols - 1].downWall = false;

        // Creating the disjoint sets
        DisjSets dsMaze = new DisjSets(numCells);

        bool done = false;
        int numUnions = 0;
        while (!done)
        {
            /********Randomly choosing two adjacent cells********/

            // Generating random number between 0 and N-1
            int cellNum = Random.Range(0, numCells);
            int row = cellNum / numCols;    // Row of number generated
            int col = cellNum % numCols;    // Column of number generated

            int pickAdj;    // Random generated number to choose adjacent cell
            int adjRow = -1;     // Row number of adjacent cell
            int adjCol = -1;     // Column number of adjacent cell

            // If cell is a border cell
            if (row == 0 || col == 0 || row == numRows - 1 || col == numCols - 1)
            {
                if (row == 0 && col == 0)  // top left corner
                {
                    // Generate a random number, either 1 or 2
                    pickAdj = Random.Range(1, 3);
                    // If 1, then right cell
                    if (pickAdj == 1)
                    {
                        adjRow = row;
                        adjCol = col + 1;
                    }
                    // If 2, then bottom cell
                    else
                    {
                        adjRow = row + 1;
                        adjCol = col;
                    }
                }
                else if (row == 0 && col == numCols - 1)  // top right corner
                {
                    // Generate a random number, either 1 or 2
                    pickAdj = Random.Range(1, 3);
                    // If 1, then bottom cell
                    if (pickAdj == 1)
                    {
                        adjRow = row + 1;
                        adjCol = col;
                    }
                    // If 2, then left cell
                    else
                    {
                        adjRow = row;
                        adjCol = col - 1;
                    }
                }
                else if (row == numRows - 1 && col == 0)  // bottom left corner
                {
                    // Generate a random number, either 1 or 2
                    pickAdj = Random.Range(1, 3);
                    // If 1, then top cell
                    if (pickAdj == 1)
                    {
                        adjRow = row - 1;
                        adjCol = col;
                    }
                    // If 2, then right cell
                    else
                    {
                        adjRow = row;
                        adjCol = col + 1;
                    }
                }
                else if (row == numRows - 1 && col == numCols - 1)  // bottom right corner
                {
                    // Generate a random number, either 1 or 2
                    pickAdj = Random.Range(1, 3);
                    // If 1, then left cell
                    if (pickAdj == 1)
                    {
                        adjRow = row;
                        adjCol = col - 1;
                    }
                    // If 2, then top cell
                    else
                    {
                        adjRow = row - 1;
                        adjCol = col;
                    }
                }
                else if (col == 0)  // left side
                {
                    // Generate a random number, either 1, 2, or 3
                    pickAdj = Random.Range(1, 4);
                    // If 1, then top cell
                    if (pickAdj == 1)
                    {
                        adjRow = row - 1;
                        adjCol = col;
                    }
                    // If 2, then right cell
                    else if (pickAdj == 2)
                    {
                        adjRow = row;
                        adjCol = col + 1;
                    }
                    // If 3, then bottom cell
                    else
                    {
                        adjRow = row + 1;
                        adjCol = col;
                    }
                }
                else if (col == numCols - 1)  // right side
                {
                    // Generate a random number, either 1, 2, or 3
                    pickAdj = Random.Range(1, 4);
                    // If 1, then bottom cell
                    if (pickAdj == 1)
                    {
                        adjRow = row + 1;
                        adjCol = col;
                    }
                    // If 2, then left cell
                    else if (pickAdj == 2)
                    {
                        adjRow = row;
                        adjCol = col - 1;
                    }
                    // If 3, then top cell
                    else
                    {
                        adjRow = row - 1;
                        adjCol = col;
                    }
                }
                else if (row == 0)  // top
                {
                    // Generate a random number, either 1, 2, or 3
                    pickAdj = Random.Range(1, 4);
                    // If 1, then right cell
                    if (pickAdj == 1)
                    {
                        adjRow = row;
                        adjCol = col + 1;
                    }
                    // If 2, then bottom cell
                    else if (pickAdj == 2)
                    {
                        adjRow = row + 1;
                        adjCol = col;
                    }
                    // If 3, then left cell
                    else
                    {
                        adjRow = row;
                        adjCol = col - 1;
                    }
                }
                else if (row == numRows - 1)  // bottom
                {
                    // Generate a random number, either 1, 2, or 3
                    pickAdj = Random.Range(1, 4);
                    // If 1, then left cell
                    if (pickAdj == 1)
                    {
                        adjRow = row;
                        adjCol = col - 1;
                    }
                    // If 2, then top cell
                    else if (pickAdj == 2)
                    {
                        adjRow = row - 1;
                        adjCol = col;
                    }
                    // If 3, then right cell
                    else
                    {
                        adjRow = row;
                        adjCol = col + 1;
                    }
                }
            }
            // Cell is an inner cell
            else
            {
                // Generate a random number, 1-4
                pickAdj = Random.Range(1, 5);
                // If 1, then top cell
                if (pickAdj == 1)
                {
                    adjRow = row - 1;
                    adjCol = col;
                }
                // If 2, then right cell
                else if (pickAdj == 2)
                {
                    adjRow = row;
                    adjCol = col + 1;
                }
                // If 3, then bottom cell
                else if (pickAdj == 3)
                {
                    adjRow = row + 1;
                    adjCol = col;
                }
                // If 4, then left cell
                else
                {
                    adjRow = row;
                    adjCol = col - 1;
                }
            }

            /**********Unioning adjacent cells if they are not in the same set**********/

            // Finding the number of the adjacency from array indices
            int adjNum = (numCols) * (adjRow) + (adjCol);
            // Finding sets
            int cellSet = dsMaze.Find(cellNum);
            int adjSet = dsMaze.Find(adjNum);

            // If cells are not in same set, union and remove wall
            //      Notice: if cells are in same set, check another pair of adjacent cells
            if (cellSet != adjSet)
            {
                // Union the two sets
                dsMaze.Union(cellSet, adjSet);
                if (row + 1 == adjRow) // If adjacent cell is the cell below
                {
                    // Removing original cell's bottom wall
                    maze[row, col].downWall = false;
                }
                else if (row - 1 == adjRow) // If adjacent cell is the cell above
                {
                    // Removing adjacent cell's bottom wall
                    maze[adjRow, adjCol].downWall = false;
                }
                else if (col + 1 == adjCol) // If adjacent cell is the cell to the right
                {
                    // Removing adjacent cell's left wall
                    maze[adjRow, adjCol].leftWall = false;
                }
                else if (col - 1 == adjCol) // If adjacent cell is the cell to the left
                {
                    // Removing original cell's left wall
                    maze[row, col].leftWall = false;
                }

                // Increment union counter
                numUnions++;
                // Once union counter = N-1, maze is done
                if (numUnions == numCells - 1)
                    done = true;
            }
        }

        return maze;
    }

    public void InstantiateMaze(MazeCell[,] maze)
    {
        // Instantiating top wall of maze
        for (int i = 0; i < numRows; i++)
        {
            Instantiate(wallPrefab,
                        new Vector3((-1 * (wallLength + 1)) + wallDepth,
                                    0,
                                    (i * (wallLength + 1)) + (wallLength / 2)),
                        Quaternion.identity);
        }

        // Instantiating right wall of maze
        for (int i = 0; i < numCols; i++)
        {
            // Instantiating left wall
            Instantiate(wallPrefab,
                        new Vector3(i * (wallLength + 1) - (wallLength / 2),
                                    0,
                                    (numCols * (wallLength + 1)) - wallDepth),
                        Quaternion.identity * Quaternion.AngleAxis(90, Vector3.up));
        }

        // Insantiating the maze itself
        for (int x = 0; x < numRows; x++)
        {
            for (int z = 0; z < numCols; z++)
            {
                MazeCell myCell = maze[x, z];
                if (myCell.downWall == true)
                {
                    // Instantiating bottom wall
                    Instantiate(wallPrefab, 
                                new Vector3((x * (wallLength + 1)) + wallDepth, 
                                            0, 
                                            (z * (wallLength + 1)) + (wallLength / 2)), 
                                Quaternion.identity);
                }
                if (myCell.leftWall == true)
                {
                    // Instantiating left wall
                    Instantiate(wallPrefab,
                                new Vector3(x * (wallLength + 1) - (wallLength / 2), 
                                            0, 
                                            (z * (wallLength + 1)) - wallDepth),
                                Quaternion.identity * Quaternion.AngleAxis(90, Vector3.up));
                }
            }
        }
    }

    /*
     * Prints the maze array (for debugging purposes)
     */
    public void PrintMaze(MazeCell[,] maze)
    {
        string maze_str = "";
        // Printing top of maze
        for (int i = 0; i < numCols; i++)
        {
            maze_str += " _";
        }
        maze_str += "\n";

        for (int r = 0; r < numRows; r++)
        {
            for (int c = 0; c < numCols; c++)
            {
                MazeCell myCell = maze[r, c];
                if (myCell.leftWall == true && myCell.downWall == true)
                    maze_str += "|_";
                else if (myCell.leftWall == false && myCell.downWall == true)
                    maze_str += " _";
                else if (myCell.leftWall == true && myCell.downWall == false)
                    maze_str += "| ";
                else
                    maze_str += "  ";
            }
            maze_str += "|\n";
        }
        maze_str += "\n\n";

        Debug.Log(maze_str);
    }
}

/*

 _ _ _ _ _ _ _ _ _ _
 _     _|_  | |  _  |
|_ _|_  |    _ _|_ _|
| | |_    |_|   |_  |
| |  _ _|_ _  |_ _ _|
| |_  |_   _| |  _  |
| |  _|  _ _  |_  | |
|_    | |  _|_| | |_|
|  _|_|   | |  _| | |
|  _|_  |_ _   _|   |
|_ _ _|_|_ _ _ _ _| |


*/