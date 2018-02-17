using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class StateController : MonoBehaviour
{

    public State currentState;
    public Enemy enemy;
    public State remainState;
    public Transform chaseTarget;


    [HideInInspector] SpriteRenderer spriteR;

    [HideInInspector] public AIPath AIPathing;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public float stateTimeElapsed;
    [HideInInspector] public float AScountdown = 0;
    [HideInInspector] public Animator anim;

    private bool aiActive;


    void Awake()
    {

        spriteR = GetComponentInChildren<SpriteRenderer>();
        spriteR.color = enemy.wColor;

        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = enemy.animator;

        AIPathing = GetComponent<AIPath>();
        AIPathing.maxSpeed = enemy.moveSpeed;
        AIPathing.endReachedDistance = enemy.attackRange -1f;
        AIPathing.rotationIn2D = true;

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
            AIPathing.enabled = true;
        }
        else
        {
            AIPathing.enabled = false;
        }
    }

    void Update()
    {
        if (!aiActive)
            return;
        
        currentState.UpdateState(this);
        enemy.UpdateAnim(this);
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
    public bool CheckAttackReady()
    {
       //Debug.Log(AScountdown );
        AScountdown -= Time.deltaTime;
        if (AScountdown <= 0)
        {
            Debug.Log("true");
            AScountdown = enemy.attackSpeed;
            return true;
        }
        else return false;
    }

public void UpdateAS()
    {
        AScountdown -= Time.deltaTime;
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    public bool checkAnimfinished()
    {
        if (enemy.attackSpeed - AScountdown > anim.GetCurrentAnimatorStateInfo(0).length)
        {
            return true;
        }
        return false;
    }

    public void getAnglePath()                                     //à garder
    {
        float angle = 0;
        if (!AIPathing.reachedEndOfPath)
        {
            Vector2 direction = AIPathing.velocity;
            angle = Vector2.Angle(direction, new Vector2(0, -1));
            if (direction.x < 0) angle = 360 - angle;
            enemy.Angle = angle;       
        }       
    }
    public void getAngleTarget()
    {
        Vector2 direction = chaseTarget.position - transform.position;
        float angle = Vector2.Angle(direction, new Vector2(0, -1));
        if (direction.x < 0) angle = 360 - angle;
        enemy.Angle = angle;
        //Debug.Log(angle);
    }
}
