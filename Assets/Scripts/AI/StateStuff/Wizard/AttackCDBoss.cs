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
        if (controller.CheckIfCountDownElapsed(controller.bossManager.anim.GetCurrentAnimatorStateInfo(0).length))
        {
            return true;
        }
        return false;
    }
}
