using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public bool walkable;
    public Vector2 worldPosition;

    public Node(bool walkabl, Vector3 worldPos)
    {
        walkable = walkabl;
        worldPosition = worldPos;
    }
}
