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
        controller.AIPathing.maxSpeed = controller.enemy.moveSpeed;
        //foreach (Collider2D targetCollider in controller.targetCollider )
        //{
        //    if (controller.enemyCollider.IsTouching(targetCollider))
        //    {
        //        controller.AIPathing.maxSpeed = 0;
        //        controller.enemy.isWalking = false;
        //    }
        //}
       
        controller.enemy.isAttacking = false;
        if (controller.AIPathing.reachedEndOfPath)
        {
            
            controller.enemy.isWalking = false;
        }
        else
        {
            controller.enemy.isWalking = true;

        }
        controller.getAngleTarget();
        controller.AIPathing.destination = controller.chaseTarget.transform.position;
    }
}