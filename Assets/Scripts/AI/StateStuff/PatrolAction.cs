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
        //controller.enemyManager.pathingUnit.RequestPath(controller.enemyManager.wayPointList[controller.enemyManager.nextWayPoint]);
        controller.enemyManager.pathingUnit.targetPosition  = controller.enemyManager.wayPointList[controller.enemyManager.nextWayPoint];

        float distance = Mathf.Abs((controller.transform.position - controller.enemyManager.pathingUnit.targetPosition).magnitude);
        if (distance < 1 && controller.enemyManager.pathingUnit.requestPath)
        {
            Debug.Log("idle");
            controller.enemyManager.idling();
        }
    }
}