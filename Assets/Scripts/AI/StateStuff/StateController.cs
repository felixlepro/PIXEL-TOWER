
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class StateController : MonoBehaviour
{
   // public AILerp AIPathing;


    public State currentState;
    //public Enemy enemy;
    public State remainState;
   
    public EnemyManager enemyManager;


    [HideInInspector] public float timeUntilNextAttack;
    [HideInInspector] public float hp;

    
    [HideInInspector]  public float stateTimeElapsed;
    [HideInInspector]   public float timeElapsed;
    [HideInInspector]  public float AScountdown = 0;
   // [HideInInspector]  public Animator anim;


    private bool aiActive = false;

    void Awake()
    {


       
      
        //wayPointList = new List<Transform>();

        // wayPointList.AddRange(GameObject.FindWithTag("waypoints").transform);                                                                                    //temporaire
        //foreach (GameObject wp in GameObject.FindGameObjectsWithTag("waypoints"))                                     //temporaire
        //{
        //    wayPointList.Add(wp.transform);
        //}
      //  enemy.controller = this;
    }

    public void SetupAI(bool aiActivationFromGameManager, List<Vector3> wayPointsFromGameManager)
    {

        enemyManager.wayPointList = wayPointsFromGameManager;
        aiActive = aiActivationFromGameManager;
       // Random.seed = System.DateTime.Now.Millisecond;
        enemyManager.nextWayPoint = Random.Range(0, enemyManager.wayPointList.Count);
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
        return (stateTimeElapsed >= duration);
    }

    //public bool CheckIfCountDownElapsed2(float duration)
    //{
    //    timeElapsed += Time.deltaTime;
    //    // Debug.Log(timeElapsed + "      " + duration  + (timeElapsed >= duration));
    //    if (timeElapsed >= duration)
    //    {
    //        timeElapsed = 0;
    //        return true;
    //    }
    //    else return false;

    //}

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

   

    //public void Death()
    //{
    //    gameObject.SetActive(false);
    //}

    //Debug.Log("true");

}

