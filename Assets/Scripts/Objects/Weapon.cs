using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
abstract public class Weapon : ScriptableObject
{
    public string weaponName;
    public Color wColor;
    public int attackDamage;
    public int cost;
    public float range;
    public float attackSpeed; //  attaque/seconde
    public RuntimeAnimatorController animator;
    public string description;
    public Sprite sprite;

    public float chargeTime;
    public int attackDamageChargedBonus;
    public float knockBackAmount;



    public int getDamage()
    {
        return attackDamage;
    }

}