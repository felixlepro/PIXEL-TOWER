



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackBoss")]
public class AttackDecisionBoss : Decision
{

    public override bool Decide(StateController controller)
    {
        Debug.Log("ca");
        return controller.enemyManager.CheckAttack();
    }
}
