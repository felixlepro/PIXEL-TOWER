﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        controller.anim.SetBool("isMoving", true);
        controller.getAngleTarget();
        controller.AIPathing.destination = controller.chaseTarget.position;
    }
}