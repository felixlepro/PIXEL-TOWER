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


    public void CreateGrid(int[,] board)
        {
            Vector2 worldBotLeft = new Vector2(-0.5f,0);        //transform.position + Vector3.left * board.GetLength(0) + Vector3.down * board.GetLength(1);
            grid = new Node[board.GetLength(0), board.GetLength(1)];
        gridSizeX = board.GetLength(0);
        gridSizeY = board.GetLength(1);
        for (int x = 0; x < gridSizeX;  x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                bool walkable = board[x, y] == 1;
                Vector2 worldPoint = worldBotLeft + Vector2.right * (2 * x + 0.5f) + Vector2.up * (2 * y + 0.5f);
                grid[x, y] = new Node(walkable, worldPoint,x,y);
                //   Debug.Log(x + "   " + y);
            }

        }
    }

    public List<Node> GetNeighbours (Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1;x <= -1;x++)
        {
            for (int y = -1; y <= -1; y++)
            {
                if (x ==0 && y ==0)     continue;

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
        float percentX = (worldPosition.x + gridSizeX / 2) / gridSizeX*2;
        float percentY = (worldPosition.y + gridSizeY / 2) / gridSizeY * 2;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> path;
        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position + new Vector3 (100,100), new Vector3(200, 200));
            Debug.Log("draew");
        if (grid != null)
        Node playerNode = NodeFromWorldPoint(player.position);
            {
            Debug.Log("draw");
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? new Color(1, 1, 1, 0.25f) : new Color(1,0,0,0.25f);
                if (playerNode == n)
                {
                    Gizmos.color = Color.green;
                }
                    if (path != null)
                    {
                        if (path.Contains(n))
                        {
                            Gizmos.color = Color.black;
                        }
                    }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (2 - .1f));
                    
                }
            }
        }
    }

