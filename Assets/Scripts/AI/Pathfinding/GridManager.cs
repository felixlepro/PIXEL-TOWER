using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public Transform player;
    //public LayerMask unwalkableMask;
    //public int gridSizeX, gridSizeY;
    // public float nodeRadius;
    Node[,] grid;
    int gridSizeX;
    int gridSizeY;

    public float nodeRadius = 0.5f;


    public void CreateGrid(int[,] board)
    {
        Vector3 worldBotLeft = new Vector3(-0.5f/nodeRadius, 0,0);        //transform.position + Vector3.left * board.GetLength(0) + Vector3.down * board.GetLength(1);

        gridSizeX = Mathf.RoundToInt(board.GetLength(0)/nodeRadius);
        gridSizeY = Mathf.RoundToInt(board.GetLength(1)/nodeRadius);

        grid = new Node[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                int nx = Mathf.FloorToInt(x * nodeRadius);
                int ny = Mathf.FloorToInt(y * nodeRadius);

                bool walkable = (board[nx, ny] == 1);
                Vector3 worldPoint = worldBotLeft + Vector3.right * (nodeRadius*2 * x + nodeRadius) + Vector3.up * (nodeRadius*2  * y + nodeRadius);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
                //   Debug.Log(x + "   " + y);
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
                    neighbours.Add(grid[checkX, checkY]);
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x  + 0.5f / nodeRadius) / (gridSizeX);
        float percentY = (worldPosition.y ) / (gridSizeY);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt((gridSizeX) * percentX);
        int y = Mathf.FloorToInt((gridSizeY) * percentY);

        if (x > gridSizeX - 1)   x = gridSizeX - 1;
        if (y > gridSizeY - 1)   y = gridSizeX - 1;
        
        return grid[x, y];
    }

    public List<Node> path; //enlever
    void OnDrawGizmos()
    {
        if (grid != null)
        {
            Node seekerNode = NodeFromWorldPoint(player.position);
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? new Color(1, 1, 1, 0.25f) : new Color(1, 0, 0, 0.5f);
                    if (seekerNode == n)
                    {
                        Gizmos.color = Color.green;
                    }
                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Debug.Log("n");

                            Gizmos.color = Color.black;
                        }
                    }

                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeRadius*2 - .1f));

                }
            }
        }
    }

}