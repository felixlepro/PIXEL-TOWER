
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
    public GameObject chaseTarget;



    [HideInInspector] public float timeUntilNextAttack;
    [HideInInspector] public float hp;

    [HideInInspector] SpriteRenderer spriteR;
    [HideInInspector]  public AIPath AIPathing;
    [HideInInspector] public List<Transform> wayPointList;
    [HideInInspector]  public int nextWayPoint;
    [HideInInspector]  public float stateTimeElapsed;
    [HideInInspector]   public float timeElapsed;
    [HideInInspector]  public float AScountdown = 0;
    [HideInInspector]  public Animator anim;
    [HideInInspector]  public Collider2D[] targetCollider;
    [HideInInspector]  public Collider2D enemyCollider;
    [HideInInspector]  public Collider2D attackHitbox;

    private bool aiActive;

    void Awake()
    {
        spriteR = gameObject.transform.Find("EnemyGraphics").gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.color = enemy.wColor;

        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = enemy.animator;

        AIPathing = GetComponent<AIPath>();
        AIPathing.maxSpeed = enemy.moveSpeed;
        AIPathing.endReachedDistance = enemy.HowLargeisHeRadius * 1.5f;
        AIPathing.slowdownDistance = enemy.HowLargeisHeRadius * 2.5f;
        AIPathing.rotationIn2D = true;

        enemyCollider = GetComponentInChildren<Collider2D>();
        targetCollider = chaseTarget.GetComponents<Collider2D>();
        attackHitbox = gameObject.transform.Find("AttackHitbox").gameObject.GetComponent<Collider2D>();
        //wayPointList = new List<Transform>();

        // wayPointList.AddRange(GameObject.FindWithTag("waypoints").transform);
        aiActive = true;                                                                                        //temporaire
        foreach (GameObject wp in GameObject.FindGameObjectsWithTag("waypoints"))                                     //temporaire
        {
            wayPointList.Add(wp.transform);
        }
        enemy.controller = this;
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
        spriteOrderInLayer();
        enemy.UpdatecurrentAttackCD();

    }
    void spriteOrderInLayer()
    {
        if (chaseTarget.transform.position.y <= transform.position.y)
        {
            spriteR.sortingOrder = -2;
        }
        else spriteR.sortingOrder = 2;
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
        //Debug.Log(stateTimeElapsed + "      " + duration);
        return (stateTimeElapsed >= duration);
    }

    public bool CheckIfCountDownElapsed2(float duration)
    {
        timeElapsed += Time.deltaTime;
        // Debug.Log(timeElapsed + "      " + duration  + (timeElapsed >= duration));
        if (timeElapsed >= duration)
        {
            timeElapsed = 0;
            return true;
        }
        else return false;

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
        Vector2 direction = chaseTarget.transform.position - transform.position;
        float angle = Vector2.Angle(direction, new Vector2(0, -1));
        if (direction.x < 0) angle = 360 - angle;
        enemy.Angle = angle;
        //Debug.Log(angle);
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }

    //Debug.Log("true");

}

