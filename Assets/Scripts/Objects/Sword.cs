using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Sword")]
public class Sword : Weapon
{
    public Sword(int damage)
    {
        attackDamage = damage;
    }

    public override void Attack()
    {
    }

    public override void setUpAS()
    {

    }
    
}