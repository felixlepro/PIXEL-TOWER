using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Actions/AttackBlob")]
public class AttackActionBlob : Action {


    public override void Act(StateController controller)
    {
        //Debug.Log("act");
        if (controller.enemy.checkIfAttackIsReady ())
        {
            controller.AIPathing.maxSpeed = controller.enemy.moveSpeed;
            //Debug.Log("rdy");
            foreach (Collider2D pc in controller.targetCollider)
            {
                if (controller.attackHitbox.IsTouching(pc))
                {
                    // Debug.Log("collided");
                    controller.enemy.mainAttack();
                    break;
                }

            }
           
        }
        else
        {
            controller.AIPathing.maxSpeed = controller.enemy.moveSpeed * (1 - (controller.enemy.timeUntilNextAttack / controller.enemy.attackSpeed)); //* (1 - (controller.enemy.timeUntilNextAttack / controller.enemy.attackSpeed));
        }
    }

}
