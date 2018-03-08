using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Sword", menuName = "Weapon/Sword/SwordFire")]
public class SwordFire : Sword
{


    public int chanceProc;
    public int burnTime;

    public SwordFire()
    {
        wColor = Color.yellow;
        attackDamage = 20;
        chanceProc = 30;
        burnTime = 4;
    }

    //public int getBurnValue()
    //{
    //   return chanceProc;

    //}


    //public void setBurnValue(int valueBurn)
    //{
    //chanceProc = valueBurn;

    //}

}