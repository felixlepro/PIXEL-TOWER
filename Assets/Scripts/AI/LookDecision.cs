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
        float distance = Vector3.Distance(controller.chaseTarget.position, controller.transform.position);
        if (distance <= controller.enemy.chaseRange)  return true;      
        else return false;
    }
}

// du bon code pour avoir une AI qui fait les chose si elle te voit

//RaycastHit hit;

//Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemy.lookRange, Color.green);

//if (Physics.SphereCast(controller.eyes.position, controller.enemy.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemy.lookRange)
//    && hit.collider.CompareTag("Player"))
//{
//    controller.chaseTarget = hit.transform;
//    return true;
//}
//else
//{
//    return false;
//}
