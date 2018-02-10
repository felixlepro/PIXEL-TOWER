﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Scan")]
public class ScanDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool noEnemyInSight = Scan(controller);
        return noEnemyInSight;
    }

    private bool Scan(StateController controller)
    {
        //controller.navMeshAgent.Stop();
        // controller.transform.Rotate(0, controller.enemy.searchingTurnSpeed * Time.deltaTime, 0);
        return true;//controller.CheckIfCountDownElapsed(controller.enemyStats.searchDuration);
    }
}