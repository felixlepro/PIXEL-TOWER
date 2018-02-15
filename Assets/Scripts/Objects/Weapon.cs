using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
abstract public class Weapon : ScriptableObject
{
    public string weaponName;
    public Color wColor;
    public int attackDamage;
    public float attackSpeed; //  attaque/seconde
    public RuntimeAnimatorController animator;

    abstract public void Attack();
    abstract public void setUpAS();

    public int GetDamage()
    {
        return attackDamage;
    }
}