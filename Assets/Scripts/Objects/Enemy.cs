using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
abstract public class Enemy : ScriptableObject
{
    public string EnemyName;
    public Color wColor;
    // public Sprite idleSprite;
    //public Sprite AttackSprite;
    public int attackDamage;
    public float attackSpeed; //  attaque/seconde
    public float lookAtDistance;
    public float chaseRange;
    public float attackRange;
    public  float moveSpeed;
    public RuntimeAnimatorController animator;
    public float lookSphereCastRadius;

    abstract public void Attack();
}
