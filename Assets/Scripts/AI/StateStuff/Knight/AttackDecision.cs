using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Attack")]
public class AttackDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return Look(controller);
    }

    private bool Look(StateController controller)
    {
        float distance = Vector3.Distance(controller.chaseTarget.transform.position, controller.transform.position);
        if (distance <= controller.enemyManager.enemy.attackRange && controller.enemyManager.checkIfAttackIsReady())
        {
            controller.enemyManager.isAttacking = true;
            controller.enemyManager.isWalking = false;
            controller.enemyManager.TryAttack();
            Debug.Log("attaking");
            return true;
        }
        else return false;
    }
}
