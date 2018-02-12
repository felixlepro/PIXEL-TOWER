using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.getAnglePath();
        // controller.target.position = controller.wayPointList[controller.nextWayPoint].position;
        controller.AIPathing.destination  = controller.wayPointList[controller.nextWayPoint].position;

        if ((controller.AIPathing.reachedEndOfPath || !controller.AIPathing.hasPath) && !controller.AIPathing.pathPending)
        {
            if (Idle(controller)) {
                controller.anim.SetBool("isMoving", true);
                controller.nextWayPoint = Random.Range(0, controller.wayPointList.Count);
                controller.AIPathing.destination = controller.wayPointList[controller.nextWayPoint].position;
                controller.AIPathing.SearchPath();
                // Debug.Log(controller.AIPath.destination);
                Debug.Log("pipi");
            }
            else
            {
                controller.anim.SetBool("isMoving", false);
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