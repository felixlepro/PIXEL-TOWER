﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return Look(controller);
    }

    private bool Look(StateController controller)
    {
        float distance = Vector3.Distance(controller.enemyManager.chaseTarget.transform.position, controller.transform.position);
        if (distance <= controller.enemyManager.chaseRange || (controller.enemyManager.gotDamaged && distance <= controller.enemyManager.chaseRange + controller.enemyManager.chaseRangeBuffer))
        {
            controller.enemyManager.currentSpeed *= controller.enemyManager.patrolSpeedChaseSpeedRatio;
            controller.enemyManager.playDun();

            return true;
        }
        else
        {
            //controller.enemyManager.enemyCollider.isTrigger = true;
            return false;
        }
    }
}
    