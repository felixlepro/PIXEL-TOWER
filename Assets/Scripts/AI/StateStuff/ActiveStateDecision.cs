﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
public class ActiveStateDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (!controller.enemyManager.chaseTarget.gameObject.activeSelf) return false;
        else if (Vector3.Distance(controller.enemyManager.chaseTarget.transform.position, controller.transform.position) > controller.enemyManager.enemy .chaseRange + controller.enemyManager.enemy.chaseRangeBuffer) return false;
        else return true;

    }


}