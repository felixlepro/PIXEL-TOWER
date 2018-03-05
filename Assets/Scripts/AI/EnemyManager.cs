using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using Pathfinding;

abstract public class EnemyManager : MonoBehaviour {

    public int hp;
    public float timeUntilNextAttack;
    [HideInInspector] public Enemy enemy;

    private Rigidbody2D enemyRigidbody;
    [HideInInspector] public Animator anim;
    [HideInInspector] public StateController controller;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isDying = false;
    [HideInInspector] public float Angle;

    [HideInInspector] public SpriteRenderer spriteR;

    [HideInInspector] public float knockBackAmount = 0;
    [HideInInspector] public float knockBackAmountOverTime = 1;
    [HideInInspector] public float knockBackAmountOverTimeMinimum = 0.85f;
    [HideInInspector] public float knockBackTime = 1;
    [HideInInspector] public Vector2 knockBackDirection;
    [HideInInspector] public Color couleurKb = Color.white;

    public GameObject chaseTarget;
    [HideInInspector]   public AILerp AIPathing;
    [HideInInspector]   public List<Transform> wayPointList;

    abstract public void TryAttack();
    abstract public void Damaged();
    abstract public void GetEnemy();

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        controller = GetComponent<StateController>();
        //controller.enemyManager = this;

        GetEnemy();

        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = enemy.animator;

        spriteR = gameObject.transform.Find("EnemyGraphics").gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.color = enemy.wColor;

        hp = enemy.maxHp;
    }

    public void Attack()
    {
        foreach (Collider2D pc in controller.targetCollider)
        {
            if (controller.attackHitbox.IsTouching(pc))
            {
                pc.gameObject.GetComponent<Player>().RecevoirDegats(enemy.attackDamage, pc.gameObject.transform.position - transform.position, enemy.knockBackAmount, enemy.immuneTime );
                resetAttackCD();
                break;
            }

        }
    }
    public void idling()
    {
        float time = Random.Range(0, 10) * enemy.idleTime;
        isWalking = false;
        Invoke("newPath", time);
       
    }
    private void newPath()
    {   
            controller.enemyManager.isWalking = true;
            controller.nextWayPoint = Random.Range(0, controller.wayPointList.Count);
        controller.AIPathing.destination = controller.wayPointList[controller.nextWayPoint].position;
            controller.AIPathing.SearchPath();
    }


    //public void checkDistanceToPlayer()
    //{
    //    if ((controller.chaseTarget.transform.position - transform.position).magnitude <= enemy.size)
    //    {
    //        //controller.AIPathing.reachedEndOfPath = true;
    //    }
  
    //}

    public void spriteOrderInLayer()
    {
        if (chaseTarget.transform.position.y <= transform.position.y)
        {
            spriteR.sortingOrder = -2;
        }
        else spriteR.sortingOrder = 2;
    }
    public void recevoirDegats(int damage, Vector3 kbDirection, float kbAmmount)
    {
        hp -= damage;
        CameraShaker.Instance.ShakeOnce(damage * 0.1f, 2.5f, 0.1f, 0.7f);
        Damaged();
        if (kbAmmount != 0)
        {
            knockBackDirection = kbDirection;
            knockBackAmount = kbAmmount;
            knockBackAmountOverTime = 0;
            StartCoroutine("KnockBack");
        }

        VerifyDeath();
    }
    public void VerifyDeath()
    {
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

    IEnumerator KnockBack()
    {
        spriteR.color = new Color(1f, 0, 0, 1f);
        while (knockBackAmountOverTime < knockBackAmountOverTimeMinimum)
        {
            float curve = (1 - knockBackAmountOverTime) * (1 - knockBackAmountOverTime);
            spriteR.color = new Color(1f, 1 - curve, 1 - curve, 1f);

            Debug.Log(knockBackDirection.normalized);

            Vector3 kb = knockBackDirection.normalized * knockBackAmount * curve * Time.deltaTime;
            enemyRigidbody .MovePosition(transform.position + kb);
            knockBackAmountOverTime += Time.deltaTime * knockBackTime;
            yield return null;
        }
        spriteR.color = new Color(1f, 1, 1, 1f);

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
