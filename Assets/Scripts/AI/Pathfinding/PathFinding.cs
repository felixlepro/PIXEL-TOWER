using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathFinding : MonoBehaviour
{
    //public Transform target;
    PathRequestManager requestManager;
    GridManager grid;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<GridManager>();
    }


    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }


    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
       
       // Debug.Log("Node     " + (startNode == targetNode));
        if (startNode != targetNode && targetNode.walkable)
        {
           
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i-1].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }


}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Diagnostics;
//using System.Linq;

//public class PathFinding : MonoBehaviour
//{

//     public Transform target; //a enlever
//    //  public float repathingRate;
//    // float time;

//    PathRequestManager requestManager;
//    GridManager grid;

//    void Awake()
//    {
//        grid = GetComponent<GridManager>();
//        requestManager = GetComponent<PathRequestManager>();
//    }

//    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
//    {
//        Stopwatch sw = new Stopwatch();
//        sw.Start();

//        Vector3[] pathWaypoints = new Vector3[0];
//        bool pathFound = false;

//        Node startNode = grid.NodeFromWorldPoint(startPos);
//        Node targetNode = grid.NodeFromWorldPoint(targetPos);

//        if (startNode.walkable && targetNode.walkable)
//        {

//            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
//            HashSet<Node> closedSet = new HashSet<Node>();
//            openSet.Add(startNode);
//            while (openSet.Count > 0)
//            {
//                Node currentNode = openSet.RemoveFirst();
//                closedSet.Add(currentNode);

//                if (currentNode == targetNode)
//                {
//                    sw.Stop();
//                    print("Path found: " + sw.ElapsedMilliseconds + "ms");
//                    pathFound = true;

//                    break;
//                }

//                foreach (Node n in grid.GetNeighbours(currentNode))
//                {
//                    if (!n.walkable || closedSet.Contains(n))
//                        continue;

//                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, n);

//                    if (newMovementCostToNeighbour < n.gCost || !openSet.Contains(n))
//                    {
//                        n.gCost = newMovementCostToNeighbour;
//                        n.hCost = GetDistance(n, targetNode);
//                        n.parent = currentNode;

//                        if (!openSet.Contains(n))
//                        {
//                            openSet.Add(n);
//                            openSet.UpdateItem(n);
//                        }
//                    }


//                }

//            }
//        }
//        yield return null;
//        if (pathFound)
//        {
//           pathWaypoints = RetracePath(startNode, targetNode);
//        }
//        requestManager.FinishedProcessingPath(pathWaypoints, pathFound);
//    }

//    Vector3[] RetracePath(Node startNode, Node endNode)
//    {
//        List<Node> path = new List<Node>();
//        Node currentNode = endNode;

//        while (currentNode != startNode)
//        {
//            path.Add(currentNode);
//            currentNode = currentNode.parent;
//        }
//        Vector3[] pathWaypoints = SimplifyPath(path);
//        pathWaypoints.Reverse();

//        return pathWaypoints;
//    }

//    Vector3[] SimplifyPath(List<Node> path)
//    {
//        List<Vector3> pathWaypoints = new List<Vector3>();
//        Vector2 directionOld = Vector2.zero;

//        for (int i = 1; i<path.Count;i++)
//        {
//            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
//            if (directionNew != directionOld)
//            {
//                pathWaypoints.Add(path[i-1].worldPosition);
//            }
//            directionOld = directionNew;
//        }
//        return pathWaypoints.ToArray();
//    }

//    int GetDistance(Node nodeA, Node nodeB)
//    {
//        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
//        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

//        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
//        return 14 * dstX + 10 * (dstY - dstX);
//    }

//    public void StartFindPath (Vector3 startPos, Vector3 targetPos)
//    {
//        StartCoroutine(FindPath(startPos, targetPos));
//    }
//}
