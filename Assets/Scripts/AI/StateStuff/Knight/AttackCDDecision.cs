﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/AtCD")]
public class AttackCDDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return CD(controller);
    }

    private bool CD(StateController controller)
    {
        if (controller.CheckIfCountDownElapsed(controller.enemyManager.anim.GetCurrentAnimatorStateInfo(0).length))
        {
            controller.enemyManager.resetAttackCD();
            return true;
        }
        return false;
    }
}
