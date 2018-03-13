using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        controller.enemyManager.isAttacking = false;
        //if (controller.enemyManager.AIPathing.reachedEndOfPath)
        //{
            
        //    controller.enemyManager.isWalking = false;
        //}
        //else
        //{
        //    controller.enemyManager.isWalking = true;

        //}

        controller.enemyManager.getAngleTarget();
        controller.enemyManager.pathingUnit.targetPosition = controller.enemyManager.chaseTarget.position;
    }
}