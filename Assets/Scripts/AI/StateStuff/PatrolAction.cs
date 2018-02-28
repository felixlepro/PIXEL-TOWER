using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        controller.getAnglePath();
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        
        // controller.target.position = controller.wayPointList[controller.nextWayPoint].position;
        controller.AIPathing.destination  = controller.wayPointList[controller.nextWayPoint].position;

        if ((controller.AIPathing.reachedEndOfPath || !controller.AIPathing.hasPath) && !controller.AIPathing.pathPending)
        {
            if (Idle(controller)) {
                //controller.anim.SetBool("isMoving", true);
                controller.enemy.isWalking = true;
                controller.nextWayPoint = Random.Range(0, controller.wayPointList.Count);
                controller.AIPathing.destination = controller.wayPointList[controller.nextWayPoint].position;
                controller.AIPathing.SearchPath();
                // Debug.Log(controller.AIPath.destination);
                Debug.Log("moving = true");
            }
            else
            {
                //controller.anim.SetBool("isMoving", false);
                controller.enemy.isWalking = false;
                Debug.Log("moving = false");
            }
        }
    }
    private bool Idle(StateController controller)
    { 
        if (controller.CheckIfCountDownElapsed(controller.enemy.idleTime))
        {
            int rng = Random.Range(0, 4);
            if (rng == 0)
            {
                return true;
            }
            else
            {
                controller.stateTimeElapsed = 0;
            }
        }
        return false;
    }
}