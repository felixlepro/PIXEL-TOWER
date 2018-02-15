﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Knight")]
public class Knight : Enemy
{

    public Knight()
    {
        hp = 100;
    }

    public override void Attack(StateController controller)
    {
        controller.anim.SetBool("isMoving", false);
        controller.getAngleTarget();

    }
    

    

}

