using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
abstract public class Enemy : ScriptableObject
{
    public string EnemyName;
    public Color wColor = Color.white;
    public int attackDamage;
    public float attackSpeed; //  attaque/seconde
    public float attackRange;
    public float moveSpeed;
    public RuntimeAnimatorController animator;
    public float idleTime;
    public int maxHp;
    public float knockBackAmount;
    
    public float chaseRange;
    public float chaseRangeBuffer;
    public float HowLargeisHeRadius;

   
}
