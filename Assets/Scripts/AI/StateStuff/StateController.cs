
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class StateController : MonoBehaviour
{

    public State currentState;
    //public Enemy enemy;
    public State remainState;
   
    public EnemyManager enemyManager;


    [HideInInspector] public float timeUntilNextAttack;
    [HideInInspector] public float hp;

    
    [HideInInspector]  public int nextWayPoint = 0;
    [HideInInspector]  public float stateTimeElapsed;
    [HideInInspector]   public float timeElapsed;
    [HideInInspector]  public float AScountdown = 0;
   // [HideInInspector]  public Animator anim;
    [HideInInspector]  public Collider2D[] targetCollider;
    [HideInInspector]  public Collider2D enemyCollider;
    [HideInInspector]  public Collider2D attackHitbox;

    private bool aiActive = false;

    void Awake()
    {


        AIPathing = GetComponent<AILerp>();
        AIPathing.speed  = enemyManager.enemy.moveSpeed;
      //  AIPathing.endReachedDistance = enemyManager.enemy.HowLargeisHeRadius * 1.5f;
        //AIPathing.slowdownDistance = enemyManager.enemy.HowLargeisHeRadius * 2.5f;
        AIPathing.rotationIn2D = true;

        enemyCollider = GetComponentInChildren<Collider2D>();
        targetCollider = chaseTarget.GetComponents<Collider2D>();
        attackHitbox = gameObject.transform.Find("AttackHitbox").gameObject.GetComponent<Collider2D>();
        //wayPointList = new List<Transform>();

        // wayPointList.AddRange(GameObject.FindWithTag("waypoints").transform);                                                                                    //temporaire
        //foreach (GameObject wp in GameObject.FindGameObjectsWithTag("waypoints"))                                     //temporaire
        //{
        //    wayPointList.Add(wp.transform);
        //}
      //  enemy.controller = this;
    }

    public void SetupAI(bool aiActivationFromGameManager, List<Transform> wayPointsFromGameManager)
    {

        wayPointList = wayPointsFromGameManager;
        aiActive = aiActivationFromGameManager;
       // Random.seed = System.DateTime.Now.Millisecond;
        nextWayPoint = Random.Range(0, wayPointList.Count);
    }

    void Update()
    {
        if (!aiActive)
            return;

        currentState.UpdateState(this);

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

    //public bool checkAnimfinished()
    //{
    //    if (enemy.attackSpeed - AScountdown > anim.GetCurrentAnimatorStateInfo(0).length)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    public void getAnglePath()                                     //à garder
    {
        float angle = 0;
        if (!AIPathing.reachedEndOfPath)
        {
            //Vector2 direction = AIPathing.velocity;
            //angle = Vector2.Angle(direction, new Vector2(0, -1));
            //if (direction.x < 0) angle = 360 - angle;
            enemyManager.Angle = angle;
        }
    }

    public void getAngleTarget()
    {
        Vector2 direction = chaseTarget.transform.position - transform.position;
        float angle = Vector2.Angle(direction, new Vector2(0, -1));
        if (direction.x < 0) angle = 360 - angle;
        enemyManager.Angle = angle;
        //Debug.Log(angle);
    }

    //public void Death()
    //{
    //    gameObject.SetActive(false);
    //}

    //Debug.Log("true");

}

