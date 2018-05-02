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
        
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (this != null)
        {
            requestProcessing = false;
            if (pathSuccessful && enabled)
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
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        path = new Vector3[0];
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                direction = currentWaypoint - transform.position;
                yield return null;

            }
        }
    }

    public void enablePathing(bool startPathing)
    {
        requestPath = true;
        if (startPathing)
        {
            PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
        }
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
