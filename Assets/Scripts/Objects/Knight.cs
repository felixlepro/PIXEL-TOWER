using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Knight")]
public class Knight : Enemy
{
    public float attackChargeTime = 0.5f;

    Knight()
    {
        maxHp = 100;

    }

}

    