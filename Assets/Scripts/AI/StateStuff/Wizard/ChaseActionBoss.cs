using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/ChaseBoss")]
public class ChaseActionBoss : Action {

    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {


        controller.enemyManager.setAnimState("Moving");
        controller.enemyManager.getAngleTarget();
        controller.enemyManager.pathingUnit.targetPosition = controller.enemyManager.chaseTarget.position;
    }
}
