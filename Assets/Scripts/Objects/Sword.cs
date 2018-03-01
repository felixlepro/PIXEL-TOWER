using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Sword")]
public class Sword : Weapon
{
    public float chargeTime = 2f;
    public int attackDamageChargedBonus;

    public override void Attack()
    {
        attackDamage = 10;
    }

    public override void setUpAS()
    {

    }
    
}