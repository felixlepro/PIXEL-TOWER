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
    public  float moveSpeed;
    public RuntimeAnimatorController animator;
    public float idleTime;
    public int hp;
    public float knockBackAmount;
    private float timeUntilNextAttack;

    public float chaseRange;
    public float chaseRangeBuffer;

    abstract public void startAttack();
    abstract public void mainAttack();
    abstract public void endAttack();

    public void recevoirDegats(int damage)
    {
        hp -= damage;
        Debug.Log(hp);
        if(hp <= 0)
        {
            isWalking = false;
            isAttacking = false;
            isDying = true;
            controller.AIPathing.enabled = false;
            controller.Invoke("Death", controller.anim.GetCurrentAnimatorClipInfo(0).Length);
        }
        else
        {
            isDying = false;
        }
    }

    private void Death()
    {
        controller.gameObject.SetActive(false);
    }

    public void UpdatecurrentAttackCD()
    {
        if (timeUntilNextAttack > 0)
        {
            timeUntilNextAttack -= Time.deltaTime;
        }
    }
    public bool checkIfAttackIsReady()
    {
        return (timeUntilNextAttack <= 0);
    }
    public void resetAttackCD()
    {
        timeUntilNextAttack = attackSpeed;
    }

    //Animation
    [HideInInspector] public StateController controller;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isDying = false;
    [HideInInspector] public float Angle;
    public float HowLargeisHeRadius;
     abstract public void UpdateAnim(StateController controller);
}
