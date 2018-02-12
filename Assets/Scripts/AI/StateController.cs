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
    [HideInInspector] public SimpleSmoothModifier sSmoothModifier;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public float stateTimeElapsed;
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
        AIPathing.endReachedDistance = enemy.attackRange;
        AIPathing.rotationIn2D = true;
        sSmoothModifier = GetComponent<SimpleSmoothModifier>();
 
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

    }

    void OnDrawGizmos()
    {
        if (currentState != null)
        {
         // spriteR.color  = currentState.sceneGizmoColor;          normalement dans le code mais ca piche des erreurs de marde meme si ya pas derreur pi que ca marche
            
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

    public void getAnglePath()                                     //à garder
    {
        float angle = transform.rotation.z;
        anim.SetFloat("Angle", angle);
        //float angle = AIPathing.SimulateRotationTowards

        //Vector2 direction = chaseTarget.position - transform.position;
        //float angle = Vector2.Angle(direction, new Vector2(0, -1));
        //if (direction.x < 0) angle = 360 - angle;
        //anim.SetFloat("Angle", angle);
        //Debug.Log(angle);
    }
    public void getAngleTarget()
    {
        Vector2 direction = chaseTarget.position - transform.position;
        float angle = Vector2.Angle(direction, new Vector2(0, -1));
        if (direction.x < 0) angle = 360 - angle;
        anim.SetFloat("Angle", angle);
    }
    }
