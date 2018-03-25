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


        controller.bossManager.updateAnimState("Move");

        controller.bossManager.getAngleTarget();
        controller.bossManager.pathingUnit.targetPosition = controller.enemyManager.chaseTarget.position;
    }
}
