using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Knight")]
public class Knight : Enemy
{

    public override void Attack(StateController controller)
    {
        controller.anim.SetBool("isMoving", false);
        controller.getAngleTarget();

    }
}

