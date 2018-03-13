using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Sword", menuName = "Weapon/Sword/SwordFire")]
public class SwordFire : Sword
{


    public int chanceBurnProc;
    public int burnTime;

    public SwordFire()
    {
        wColor = Color.yellow;
        attackDamage = 20;
        chanceBurnProc = 30;
        burnTime = 4;
    }

   

}