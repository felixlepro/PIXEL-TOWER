using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class AttackAction : Action
{
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller)
    {
            controller.enemy.isAttacking = true;
            controller.enemy.isWalking = false;
            controller.enemy.Attack();
              //Debug.Log("attaking");
    }
}