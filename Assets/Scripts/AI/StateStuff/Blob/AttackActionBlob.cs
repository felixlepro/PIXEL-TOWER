using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Actions/AttackBlob")]
public class AttackActionBlob : Action {


    public override void Act(StateController controller)
    {
        if (controller.enemy.checkIfAttackIsReady ())
        {
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
    }

}
