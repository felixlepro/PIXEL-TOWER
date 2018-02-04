using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Sword")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public Sprite idleSprite;
    public Sprite attackSprite;
    public Color wColor;
    public int attackDamage;
    public float attackSpeed; //  attaque/seconde
    public RuntimeAnimatorController animator;


    public void  attack(Rigidbody2D  rb)
    {

    }

    public void setUpAS() {

    }

}