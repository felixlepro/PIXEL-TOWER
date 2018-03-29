using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackCDBoss")]
public class AttackCDBoss : Decision
{
    public override bool Decide(StateController controller)
    {
        return CD(controller);
    }

    private bool CD(StateController controller)
    {
        if (controller.enemyManager.checkIfAttackIsReady())//controller.CheckIfCountDownElapsed(controller.enemyManager.anim.GetCurrentAnimatorStateInfo(0).length))
        {
            return true;
        }
        return false;
    }
}
