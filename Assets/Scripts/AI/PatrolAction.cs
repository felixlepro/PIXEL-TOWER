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
        // controller.target.position = controller.wayPointList[controller.nextWayPoint].position;
        controller.AIPath.destination  = controller.wayPointList[controller.nextWayPoint].position;

        if ((controller.AIPath.reachedEndOfPath || !controller.AIPath.hasPath) && !controller.AIPath.pathPending)
        {
            controller.nextWayPoint = Random.Range(0, controller.wayPointList.Count);
            controller.AIPath.destination = controller.wayPointList[controller.nextWayPoint].position;
            controller.AIPath.SearchPath();
           // Debug.Log(controller.AIPath.destination);
            Debug.Log("pipi");
        }
    }
}