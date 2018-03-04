using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Blob")]
public class Blob : Enemy
{
    public float hpLostOnAttack;

    Blob()
    {
        maxHp = 100;
    }
}

 
