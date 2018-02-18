using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ActiveState")]
public class ActiveStateDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (!controller.chaseTarget.gameObject.activeSelf) return false;
        else if (Vector3.Distance(controller.chaseTarget.transform.position, controller.transform.position) > controller.enemy.chaseRange + controller.enemy.chaseRangeBuffer) return false;
        else return true;

    }


}