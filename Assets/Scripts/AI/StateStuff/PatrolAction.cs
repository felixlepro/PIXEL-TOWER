using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        controller.enemyManager.getAnglePath();
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {

        controller.enemyManager.AIPathing.destination  = controller.enemyManager.wayPointList[controller.nextWayPoint].position;

        if ((controller.enemyManager.AIPathing.reachedEndOfPath || !controller.enemyManager.AIPathing.hasPath) && !controller.enemyManager.AIPathing.pathPending)
        {
           // Debug.Log("goIdle");
            controller.enemyManager.idling();
        }
    }
}