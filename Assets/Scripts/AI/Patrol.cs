using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : StateMachineBehaviour {

    GameObject NPC;
    GameObject[] wayPoints;
    int currentWP;
    float patrolSpeedMultiplier = 1.0f;
    EnemyManager EMtoPatrol;
    Animator anim;

    void Awake()
    {
        wayPoints = GameObject.FindGameObjectsWithTag("waypoints");
        Debug.Log(wayPoints[0].transform.position.x);
        
    }


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        NPC = animator.gameObject;
        currentWP = 0;
        EMtoPatrol = NPC.GetComponentInChildren<EnemyManager>();
        anim.runtimeAnimatorController = EMtoPatrol.enemy.animator;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (wayPoints.Length == 0) return;
        if (Vector3.Distance(wayPoints[currentWP].transform.position,NPC.transform.position)<3.0f)
        {
            currentWP++;
                if(currentWP >= wayPoints.Length)
            {
                currentWP = 0;
            }
        }

        getAngle();
        walk();

	}
    void getAngle()
    {
        Vector2 direction = wayPoints[currentWP].transform.position - NPC.transform.position;
        float angle = Vector2.Angle(direction, new Vector2(0, -1));
        if (direction.x < 0) angle = 360 - angle;
        anim.SetFloat("Angle", angle);
        //Debug.Log(angle);
    }
    void isMoving(bool moving)
    {
        anim.SetBool("isMoving", moving);
    }
    void walk()
    {
        NPC.transform.position = Vector3.Lerp(NPC.transform.position, wayPoints[currentWP].transform.position, Time.deltaTime*    EMtoPatrol.enemy.moveSpeed * patrolSpeedMultiplier / (Vector3.Distance(wayPoints[currentWP].transform.position, NPC.transform.position)));
        isMoving(true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

}
