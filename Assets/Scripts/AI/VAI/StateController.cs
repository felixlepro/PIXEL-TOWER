using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class StateController : MonoBehaviour
{

    public State currentState;
    public Enemy enemy;
    public Transform eyes;
    public State remainState;

    SpriteRenderer spriteR;

    [HideInInspector] public AIPath AIPath;
    //[HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Transform chaseTarget;
    [HideInInspector] public float stateTimeElapsed;

    private bool aiActive;


    void Awake()
    {
        spriteR = GetComponentInChildren<SpriteRenderer>();
        AIPath = GetComponent<AIPath>();
        //navMeshAgent = GetComponent<NavMeshAgent>();
        //wayPointList = new List<Transform>();
        // wayPointList.AddRange(GameObject.FindWithTag("waypoints").transform);
        aiActive = true;                                                                                        //temporaire
        foreach (GameObject wp in GameObject.FindGameObjectsWithTag("waypoints"))                                     //temporaire
        {
            wayPointList.Add(wp.transform);
        }
    }

    public void SetupAI(bool aiActivationFromGameManager, List<Transform> wayPointsFromGameManager)
    {

        wayPointList = wayPointsFromGameManager;
        aiActive = aiActivationFromGameManager;
        if (aiActive)
        {
            AIPath.enabled = true;
        }
        else
        {
            AIPath.enabled = false;
        }
    }

    void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            spriteR.color  = currentState.sceneGizmoColor;
            //Gizmos.color = currentState.sceneGizmoColor;
            //Gizmos.DrawWireSphere(eyes.position, enemy.lookSphereCastRadius);
        }
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }
}
