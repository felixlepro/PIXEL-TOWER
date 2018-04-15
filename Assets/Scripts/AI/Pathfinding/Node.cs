using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

    public bool walkable;
    public bool targetOnlyWalkable;
   public Vector2 worldPosition;
    public int gridX, gridY;
    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node(bool walkabl, Vector2 worldPos, int gridXx, int gridYy)
    {
        walkable = walkabl;
        worldPosition = worldPos;
        gridX = gridXx;
        gridY = gridYy;
       if (walkable) GameManager.instance.wayPointList.Add(worldPos);
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

}

