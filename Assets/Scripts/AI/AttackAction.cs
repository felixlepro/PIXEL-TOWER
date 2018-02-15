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

        if(Vector3.Distance(controller.chaseTarget.position, controller.transform.position) <= controller.enemy.attackRange 
            //&& controller.CheckIfCountDownElapsed(controller.enemy.attackSpeed)
           )
        {
            controller.anim.SetBool("isAttacking", true);
            controller.enemy.Attack(controller);
        }
        else controller.anim.SetBool("isAttacking", false);
    }
}