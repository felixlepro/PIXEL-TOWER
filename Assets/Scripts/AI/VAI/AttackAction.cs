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
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemy.attackRange, Color.red);

        if (Physics.SphereCast(controller.eyes.position, controller.enemy.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemy.attackRange)
            && hit.collider.CompareTag("Player"))
        {
            if (controller.CheckIfCountDownElapsed(controller.enemy.attackSpeed))
            {
               // controller.tankShooting.Fire(controller.enemy.attackDamage, controller.enemy.attackSpeed);    Super important à changer
            }
        }
    }
}