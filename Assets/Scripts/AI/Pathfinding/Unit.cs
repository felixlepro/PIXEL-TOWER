using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    [HideInInspector]   public float speed;
    [HideInInspector]   public Vector3 targetPosition;
    public float repathRate;
    [HideInInspector] public bool requestPath = true;
    [HideInInspector]
    public bool requestProcessing = false;

    Vector3[] path;
    int targetIndex;
    float time = 0;

    [HideInInspector] public Vector3 direction;

    void Update()
    {
        time += Time.deltaTime;
        if ((time > repathRate && requestPath)&& !requestProcessing &&(path == null || path.Length <= 0 || path[path.Length - 1] != targetPosition))
        {
            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
            requestProcessing = true;
            time = 0;
        }
        //if (path == null || path.Length <= 0)
        //{
        //    if (time > repathRate && requestPath)
        //    {
        //        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
        //        time = 0;
        //    }
        //}
        //else
        //{
        //    if (time > repathRate && requestPath &&  path[path.Length - 1] != targetPosition)
        //    {
        //        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
        //        time = 0;
        //    }
        //}
        
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (this != null)
        {
            requestProcessing = false;
            if (pathSuccessful)
            {
                
                path = newPath;
                targetIndex = 0;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }
    }
    public void ResetPF()
    {
        StopCoroutine("FollowPath");
        targetPosition = GameManager.instance.player.transform.position;
    }
    IEnumerator FollowPath()
    {
       // Debug.Log(path.Length);
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
               // Debug.Log(currentWaypoint + "   transform" + transform.position);
                if (transform.position == currentWaypoint)
                {
                    
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                       // Debug.Log("leave");
                        targetIndex = 0;
                        path = new Vector3[0];
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
              //  Debug.Log("ok");
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                direction = currentWaypoint - transform.position;
                yield return null;

            }
        }
       // Debug.Log("end");
    }

    public void enablePathing()
    {
        requestPath = true;
        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
        time = 0;
    }
    public void disablePathing()
    {
        requestPath = false;
        StopCoroutine("FollowPath");
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(path[i], Vector3.one * (0.5f - .03f));

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
    public bool IsWalking()
    {
        if (path == null || path.Length ==0)
        {
            return false;
        }
        return true;
    }
}
//using UnityEngine;
 //using System.Collections;

//public class Unit : MonoBehaviour
//{
//    [HideInInspector] public float speed;
//    [HideInInspector] public Vector3 targetPosition;
//    public float repathRate;
//    [HideInInspector] public bool requestPath = true;
//    [HideInInspector] public bool hasPath = false;
//    [HideInInspector] public Vector3 currentPathTargetPos;
//    PathFinding pathfinding;

//    Vector3[] path;
//    int targetIndex;
//    float time = 0;

//    [HideInInspector] public Vector3 direction;


//    const float minPathUpdateTime = .2f;
//    const float pathUpdateMoveThreshold = .5f;

//    //void Update()
//    //{
//    //    if (requestPath) StartCoroutine(UpdatePath());
//    //}
//    private void Start()
//    {
//        currentPathTargetPos = Vector3.zero;
//        pathfinding = GameObject.Find("GameManager").GetComponent<PathFinding>();
//    }
//    void Update()
//    {
//        //time += Time.deltaTime;
//        if (requestPath && currentPathTargetPos != targetPosition && !hasPath)// && time > repathRate )
//        {
//            hasPath = true;
//            currentPathTargetPos = targetPosition;
//            Debug.Log("requestPath");
//            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
//            time = 0;
//        }
//    }
//    public void RequestPath(Vector3 position)
//    {
//        targetPosition = position;
//        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
//    }
//    //IEnumerator UpdatePath()
//    //{

//    //    if (Time.timeSinceLevelLoad < .3f)
//    //    {
//    //        yield return new WaitForSeconds(.3f);
//    //    }
//    //    PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);

//    //    float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
//    //    Vector3 targetPosOld = targetPosition;

//    //    while (true)
//    //    {
//    //        yield return new WaitForSeconds(minPathUpdateTime);
//    //        if ((targetPosition - targetPosOld).sqrMagnitude > sqrMoveThreshold)
//    //        {
//    //            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
//    //            targetPosOld = targetPosition;
//    //        }
//    //    }
//    //}
//    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
//    {
//        if (pathSuccessful)
//        {
//            path = newPath;
//            targetIndex = 0;
//            Debug.Log("pathSuccess");
//            StopCoroutine(FollowPath());
//            StartCoroutine(FollowPath());
//        }
//    }

//    IEnumerator FollowPath()
//    {
//        if (path.Length > 0)
//        {
//            Vector3 currentWaypoint = path[0];
//            while (true)
//            {
//                if (transform.position == currentWaypoint)
//                {
//                    targetIndex++;
//                    if (this.gameObject.name == "EnemyBlob (1)") Debug.Log(targetIndex + "     afterIndex");
//                    //if (this.gameObject.name == "EnemyBlob (1)") Debug.Log(path.Length);
//                    if (targetIndex >= path.Length)
//                    {
//                        Debug.Log("wtf");
//                        targetIndex = 0;
//                        path = new Vector3[0];
//                        yield break;
//                    }
//                    currentWaypoint = path[targetIndex];
//                }
//                //Debug.Log(transform.position + "        " + currentWaypoint);
//                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
//                direction = currentWaypoint - transform.position;
//                //if (this.gameObject.name == "EnemyBlob (1)") Debug.Log(targetIndex + "      beforeReturn");
//                yield return null;
//                //if (this.gameObject.name == "EnemyBlob (1)") Debug.Log(targetIndex + "      afterReturn");

//            }
//        }
//    }

//    public void enablePathing()
//    {
//        requestPath = true;
//        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
//        time = 0;
//    }
//    public void disablePathing()
//    {
//        requestPath = false;
//        StopCoroutine("FollowPath");
//    }

//    public void OnDrawGizmos()
//    {
//        if (path != null)
//        {
//            for (int i = targetIndex; i < path.Length; i++)
//            {
//                Gizmos.color = Color.white;
//                Gizmos.DrawCube(path[i], Vector3.one);

//                if (i == targetIndex)
//                {
//                    Gizmos.DrawLine(transform.position, path[i]);
//                }
//                else
//                {
//                    Gizmos.DrawLine(path[i - 1], path[i]);
//                }
//            }
//        }
//    }
//}
////IEnumerator FollowPath()
////{

////    bool followingPath = true;
////    int pathIndex = 0;
////    transform.LookAt(path.lookPoints[0]);

////    float speedPercent = 1;

////    while (followingPath)
////    {
////        Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
////        while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
////        {
////            if (pathIndex == path.finishLineIndex)
////            {
////                followingPath = false;
////                break;
////            }
////            else
////            {
////                pathIndex++;
////            }
////        }

////        if (followingPath)
////        {

////            if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
////            {
////                speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
////                if (speedPercent < 0.01f)
////                {
////                    followingPath = false;
////                }
////            }

////            Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
////            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
////            transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
////        }

////        yield return null;

////    }
////}