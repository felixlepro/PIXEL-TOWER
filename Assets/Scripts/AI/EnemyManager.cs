using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyManager : MonoBehaviour {

    public int hp;
    public float timeUntilNextAttack;
    public Enemy enemy;

    [HideInInspector] public Animator anim;
    [HideInInspector] public StateController controller;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isDying = false;
    [HideInInspector] public float Angle;

    [HideInInspector] public SpriteRenderer spriteR;

    abstract public void TryAttack();

    void Start()
    {
        controller = GetComponent<StateController>();
        //controller.enemyManager = this;

        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = enemy.animator;

        spriteR = gameObject.transform.Find("EnemyGraphics").gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.color = enemy.wColor;

        hp = enemy.maxHp;
    }
    private void Update()
    {
        
    }

   public void spriteOrderInLayer()
    {
        if (controller.chaseTarget.transform.position.y <= transform.position.y)
        {
            spriteR.sortingOrder = -2;
        }
        else spriteR.sortingOrder = 2;
    }
    public void recevoirDegats(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            isWalking = false;
            isAttacking = false;
            isDying = true;
            controller.AIPathing.enabled = false;
            Invoke("Death", anim.GetCurrentAnimatorClipInfo(0).Length);
        }
        else
        {
            isDying = false;
        }
    }

    private void Death()
    {
        this.gameObject.SetActive(false);
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
        timeUntilNextAttack = enemy.attackSpeed;
    }
}
