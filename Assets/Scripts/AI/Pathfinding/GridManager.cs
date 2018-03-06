using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    //public LayerMask unwalkableMask;
    //public int gridSizeX, gridSizeY;
    // public float nodeRadius;
    Node[,] grid;


    private void Start()
    {
        //int[,] c= new int[2, 15] {{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 },
        //{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 }};
        //CreateGrid(c);

    }
    public void CreateGrid(int[,] board)
        {
            Vector2 worldBotLeft = new Vector2(0,0);        //transform.position + Vector3.left * board.GetLength(0) + Vector3.down * board.GetLength(1);
            grid = new Node[board.GetLength(0), board.GetLength(1)];

        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                bool walkable = board[x, y] == 1;
                Vector2 worldPoint = worldBotLeft + Vector2.right * (2 * x + 0.5f) + Vector2.up * (2 * y + 0.5f);
                grid[x, y] = new Node(walkable, worldPoint);
                //   Debug.Log(x + "   " + y);
            }

        } }



        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position + new Vector3 (100,100), new Vector3(200, 200));
        Debug.Log("draew");
        if (grid != null)
            {
            Debug.Log("draw");
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? new Color(1, 1, 1, 0.25f) : new Color(1,0,0,0.25f);
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (2 - .1f));
                }
            }
        }
    }

