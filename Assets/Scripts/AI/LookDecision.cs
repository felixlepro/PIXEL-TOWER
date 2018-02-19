using System.Collections;
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
        float distance = Vector3.Distance(controller.chaseTarget.transform.position, controller.transform.position);
        if (distance <= controller.enemy.chaseRange)  return true;      
        else return false;
    }
}
