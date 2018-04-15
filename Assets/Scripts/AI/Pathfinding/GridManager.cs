using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    //public LayerMask unwalkableMask;
    //public int gridSizeX, gridSizeY;
    // public float nodeRadius;
    Node[,] grid;
    int gridSizeX;
    int gridSizeY;
    public bool enableGizmos;
    Vector3 worldBotLeft;
    Vector2 gridWorldSize;
    public float nodeRadius;
    //public int NodeContour;


    public void CreateGrid(int[,] board)
    {
        gridSizeX = Mathf.RoundToInt((board.GetLength(0)) / nodeRadius);
        gridSizeY = Mathf.RoundToInt((board.GetLength(1)) / nodeRadius);
        gridWorldSize = new Vector2(gridSizeX * nodeRadius * 2, gridSizeY * nodeRadius * 2);
        grid = new Node[gridSizeX, gridSizeY];
        worldBotLeft = new Vector3(-1, -1, 0);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                int nx = Mathf.FloorToInt(x * nodeRadius);
                int ny = Mathf.FloorToInt(y * nodeRadius);
                bool walkable = (board[nx, ny] == 1);

                if (board[nx, ny] == 3 && Mathf.RoundToInt(y * nodeRadius*4) != ny*4)
                {
                    //Debug.Log(Mathf.RoundToInt(y * nodeRadius * 2) + "      " + ny * 2);
                    walkable = true;
                }
                else if (board[nx, ny] == 2 && Mathf.RoundToInt(y * nodeRadius * 2) == ny * 2) //fait en sorte que la partie du haut des tiles des chests soit walkable
                {
                    walkable = true;
                }

                //if (walkable)
                //{
                //    for (int vx = -NodeContour; vx <= NodeContour; vx += NodeContour)
                //    {
                //        for (int vy = -NodeContour; vy <= NodeContour; vy += NodeContour)
                //        {
                //            int nvx = Mathf.FloorToInt((x + vx) * nodeRadius);
                //            int nvy = Mathf.FloorToInt((y + vy) * nodeRadius);

                //            if (board[nvx, nvy] == 0)
                //            {
                //                walkable = false;
                //                break;
                //            }
                //        }
                //    }
                //}

                Vector3 worldPoint = worldBotLeft + Vector3.right * (nodeRadius * 2 * x + nodeRadius) + Vector3.up * (nodeRadius * 2 * y + nodeRadius);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }

        }
    }




    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeY && checkY >= 0 && checkY < gridSizeY)
                {
                    if (grid[checkX, checkY].walkable && (grid[node.gridX,checkY].walkable || grid[checkX, node.gridY].walkable))
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                   
                }
                   
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        //float percentX = (worldPosition.x - worldBotLeft.x)/ ((gridSizeX)*nodeRadius*2);
        //float percentY = (worldPosition.y - worldBotLeft.y)/ ((gridSizeY)*nodeRadius*2);


        //percentX = Mathf.Clamp01(percentX);
        //percentY = Mathf.Clamp01(percentY);

        //int x = Mathf.FloorToInt((gridSizeX) * percentX);
        //int y = Mathf.FloorToInt((gridSizeY) * percentY);

        //if (x > gridSizeX - 1)   x = gridSizeX - 1;
        //if (y > gridSizeY - 1)   y = gridSizeY - 1;

        //return grid[x, y];

        Vector3 localPosition = worldPosition - worldBotLeft;

        float percentX = (localPosition.x) / gridWorldSize.x;
        float percentY = (localPosition.y) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    void OnDrawGizmos()
    {

        //if (grid != null && enableGizmos)
        //{
        //    {
        //        foreach (Node n in grid)
        //        {
        //            Gizmos.color = new Color(1, 0, 0, 0.25f);
        //            if (n.walkable)
        //            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeRadius * 2 - .03f));
                    

        //        }
        //    }
        //}

    }
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

}


//if (walkable)               
//                {
//                    int nx_p = Mathf.FloorToInt((x + NodeContour) * nodeRadius);
//int nx_m = Mathf.FloorToInt((x - NodeContour) * nodeRadius);
//int ny_p = Mathf.FloorToInt((y + NodeContour) * nodeRadius);
//int ny_m = Mathf.FloorToInt((y - NodeContour) * nodeRadius);

//                    if (board.GetLength(0)-1 <= nx_p)
//                    {
//                        if(board[nx_p, ny] == 0)
//                        { 
//                            walkable = false;
//                        }
//                    }
//                    if (board.GetLength(0)-1 >= nx_m)
//                    {
//                        if (board[nx_m, ny] == 0)
//                        {
//                            walkable = false;
//                        }
//                    }
//                    if (board.GetLength(1)-1 <= ny_p)
//                    {
//                   //     Debug.Log("ny_p="+ ny_p + "      ny=" + ny + "      board=" + board.GetLength(1));

//                        if (board[nx, ny_p] == 0)
//                        {
//                            walkable = false;
//                        }
//                    }
//                    if (board.GetLength(1)-1 >= ny_m)
//                    {
//                        if (board[nx, ny_m] == 0)
//                        {
//                            walkable = false;
//                        }
//                    }

//                }

// int nx_p = Mathf.FloorToInt((x + NodeContour) * nodeRadius);
// int nx_m = Mathf.FloorToInt((x - NodeContour) * nodeRadius);
// int ny_p = Mathf.FloorToInt((y + NodeContour) * nodeRadius);
// int ny_m = Mathf.FloorToInt((y - NodeContour) * nodeRadius);


// if (board[nx_p, ny] == 0)
// {
//     walkable = false;
// }
//else  if (board[nx_m, ny] == 0)
// {
//     walkable = false;
// }
//else  if (board[nx, ny_p] == 0)
// {
//     walkable = false;
// }
//else  if (board[nx, ny_m] == 0)
// {
//     walkable = false;
// }