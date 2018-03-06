using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public bool walkable;
   public Vector2 worldPosition;
    public int gridX, gridY;
    public int gCost;
    public int hCost;
    public Node parent;


    public Node(bool walkabl, Vector2 worldPos, int gridXx, int gridYy)
    {
        walkable = walkabl;
        worldPosition = worldPos;
        gridX = gridXx;
        gridY = gridYy;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
