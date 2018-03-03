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
        
        controller.AIPathing.destination  = controller.wayPointList[controller.nextWayPoint].position;

        if ((controller.AIPathing.reachedEndOfPath || !controller.AIPathing.hasPath) && !controller.AIPathing.pathPending)
        {
            Debug.Log("goIdle");
            controller.enemyManager.idling();
        }
    }
}