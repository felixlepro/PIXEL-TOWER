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
        float distance = Vector3.Distance(controller.enemyManager.chaseTarget.transform.position, controller.transform.position);
        if (distance <= controller.enemyManager.attacks[0].attackRange && controller.enemyManager.checkIfAttackIsReady())
        {
            controller.enemyManager.setAnimState("Attacking");
            controller.enemyManager.TryAttack();
           // Debug.Log("attaking");
            return true;
        }
        else return false;
    }
}
