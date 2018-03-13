using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Sword", menuName = "Weapon/Sword/SwordIce")]
public class SwordIce : Sword
{
    public int chanceSlowProc;
    public int slowTime;
    public float slowMultip;
   
    public SwordIce()
    {
        wColor = Color.blue;
        attackDamage = 20;
        slowTime = 3;
        slowMultip = 0.65;
        chanceSlowProc = 48;
    }

}