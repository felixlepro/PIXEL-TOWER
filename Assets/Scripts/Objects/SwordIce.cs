using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Sword", menuName = "Weapon/Sword/SwordIce")]
public class SwordIce : Sword
{
    public SwordIce()
    {
        wColor = Color.blue;
        attackDamage = 20;
    }

}