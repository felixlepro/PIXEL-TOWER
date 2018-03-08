using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathFinding : MonoBehaviour
{

    public Transform seeker, target; //a enlever
    public float repathingRate;
    float time;

    GridManager grid;

    void Awake()
    {
        grid = GetComponent<GridManager>();
    }

    void Update()
    {
        if (time <= 0)
        {
            time = repathingRate;
            FindPath(seeker.position, target.position);
        }
        time -= Time.deltaTime;
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + "ms");
                RetracePath(startNode, targetNode);

                return;
            }

            foreach (Node n in grid.GetNeighbours(currentNode))
            {
                if (!n.walkable || closedSet.Contains(n))
                    continue;

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, n);

                if (newMovementCostToNeighbour < n.gCost || !openSet.Contains(n))
                {
                    n.gCost = newMovementCostToNeighbour;
                    n.hCost = GetDistance(n, targetNode);
                    n.parent = currentNode;

                    if (!openSet.Contains(n))
                    {
                        openSet.Add(n);
                        openSet.UpdateItem(n);
                    }
                }


            }

        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;


    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
