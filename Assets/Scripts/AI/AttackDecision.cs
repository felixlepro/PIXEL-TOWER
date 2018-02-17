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
        float distance = Vector3.Distance(controller.chaseTarget.position, controller.transform.position);
        if (distance <= controller.enemy.attackRange && controller.CheckAttackReady()) return true;
        else return false;
    }
}
