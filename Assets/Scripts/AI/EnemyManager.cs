using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

abstract public class EnemyManager : MonoBehaviour {

    public string EnemyName;
    public Color wColor = Color.white;
    public int attackDamage;
    public float attackSpeed; //  attaque/seconde
    public float attackRange;
    public float maxMoveSpeed;
    public RuntimeAnimatorController animator;
    public float idleTime;
    public int maxHp;
    public float maxKnockBackAmount;
    public float gettingKnockedBackAmount;
    public float immuneTime;

    public float chaseRange;
    public float chaseRangeBuffer;
    public float size;
    public AudioClip dun;
    public bool hitAWall = false;


    public int hp;
    [HideInInspector] public bool isRooted = false;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float timeUntilNextAttack;

    [HideInInspector] public Rigidbody2D enemyRigidbody;
    [HideInInspector] public Animator anim;
    [HideInInspector] public StateController controller;
    [HideInInspector] public SpriteRenderer spriteR;

    [HideInInspector] public Collider2D[] targetCollider;
    [HideInInspector] public Collider2D enemyCollider;
    [HideInInspector] public Collider2D[] attackHitbox;

    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isDying = false;
    [HideInInspector] public float Angle;

    [HideInInspector] public float knockBackAmount = 0;
    [HideInInspector] const float knockBackAmountOverTimeMinimum = 0.85f;
    [HideInInspector] const float knockBackTime = 1;
    [HideInInspector] public Vector2 knockBackDirection;
    [HideInInspector] public Color couleurKb = Color.white;

    [HideInInspector] public Transform chaseTarget;

    [HideInInspector]   public List<Vector3> wayPointList;
    [HideInInspector] public int nextWayPoint = 0;

    [HideInInspector] public Unit pathingUnit;

    abstract public void TryAttack();
    abstract public void Damaged();
    abstract public void AttackSuccessful();
    abstract public void UpdateAnim();

    void Start()
    {
        currentSpeed = maxMoveSpeed;
        pathingUnit = GetComponent<Unit>();
        pathingUnit.speed = currentSpeed;

        chaseTarget = GetComponentInParent<PlayerTarget>().playerTarget;
        hp = maxHp;

        enemyRigidbody = GetComponent<Rigidbody2D>();
        controller = GetComponent<StateController>();
        //controller.enemyManager = this;

        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = animator;

        spriteR = gameObject.transform.Find("EnemyGraphics").gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.color = wColor;

        enemyCollider = GetComponentInChildren<Collider2D>();
        targetCollider = chaseTarget.GetComponents<Collider2D>();
        attackHitbox = gameObject.transform.Find("AttackHitbox").gameObject.GetComponents<Collider2D>();

        
    }
    private void Update()
    {
        if (isRooted)     pathingUnit.speed = 0;    
       else               pathingUnit.speed = currentSpeed;
        if (!isAttacking)      anim.speed = currentSpeed / maxMoveSpeed;
        UpdateAnim();
        spriteOrderInLayer();
        UpdatecurrentAttackCD();

    }
    
    public void Attack()
    {
        if (!targetCollider[0].gameObject.GetComponent<Player>().immune)
        {
            foreach (Collider2D pc in targetCollider)
            {
                foreach (Collider2D ah in attackHitbox)
                {
                    if (ah.IsTouching(pc))
                    {
                        pc.gameObject.GetComponent<Player>().RecevoirDegats(attackDamage, pc.gameObject.transform.position - transform.position, maxKnockBackAmount, immuneTime);
                        AttackSuccessful();
                        resetAttackCD();
                        break;
                    }
                }
            }
        }
    }
    public void idling()
    {
        float time = Random.Range(1, 5) * idleTime;
        isWalking = false;
        pathingUnit.disablePathing();
        Debug.Log("invoke:" + time);
        //newPath();
        Invoke("newPath", time);    
    }
    private void newPath()
    {
        Debug.Log("newpath");
            isWalking = true;
            nextWayPoint = Random.Range(0, wayPointList.Count-1);

        //pathingUnit.RequestPath(wayPointList[nextWayPoint]);
        pathingUnit.targetPosition = wayPointList[nextWayPoint];        
        pathingUnit.enablePathing();

        // Debug.Log(wayPointList[nextWayPoint]);
        //Debug.Log("EnabledPathing");
        //AIPathing.destination = wayPointList[controller.nextWayPoint].position;
        //AIPathing.SearchPath();
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
        knockBackAmount = kbAmmount * gettingKnockedBackAmount;
        Damaged();  
        if (knockBackAmount != 0)
        {
            knockBackDirection = kbDirection;
            //knockBackAmountOverTime = 0;
            StartCoroutine("KnockBack");
        }
        else StartCoroutine("RedOnly");

        VerifyDeath();
    }
    public void VerifyDeath()
    {
        if (hp <= 0)
        {
            currentSpeed = 0;
            isWalking = false;
            isAttacking = false;
            isDying = true;
            UpdateAnim();
            Invoke("Death", anim.GetCurrentAnimatorClipInfo(0).Length*anim.speed);
        }
        else
        {
            isDying = false;
        }
    }
   void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "obstacle" && !other.isTrigger)
        {
            hitAWall = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        hitAWall = false;
    }

    public IEnumerator RedOnly()
    {
        float kbAmountOverTime = 0;
        spriteR.color = new Color(1f, 0, 0, 1f);
        while (kbAmountOverTime < knockBackAmountOverTimeMinimum)
        {
            if (!hitAWall)
            {
                float curve = (1 - kbAmountOverTime) * (1 - kbAmountOverTime);
                spriteR.color = new Color(1f, 1 - curve, 1 - curve, 1f);

                kbAmountOverTime += Time.deltaTime * knockBackTime * 1.75f;
            }
            yield return null;
        }
        spriteR.color = new Color(1f, 1, 1, 1f);
    }
    public IEnumerator KnockBack()
    {
        yield return new WaitForFixedUpdate();
        pathingUnit.speed = 0;
        pathingUnit.disablePathing();
        float kbAmountOverTime = 0;
        spriteR.color = new Color(1f, 0, 0, 1f);
        while (kbAmountOverTime < knockBackAmountOverTimeMinimum)
        {
            float curve = (1 - kbAmountOverTime) * (1 - kbAmountOverTime);
            spriteR.color = new Color(1f, 1 - curve, 1 - curve, 1f);

          //  Debug.Log(knockBackDirection.normalized);

            Vector3 kb = knockBackDirection.normalized * knockBackAmount * curve * Time.deltaTime;

            enemyRigidbody.MovePosition(transform.position + kb);
            //transform.position = Vector3.MoveTowards(transform.position, transform.position+kb, Time.deltaTime);
            kbAmountOverTime += Time.deltaTime * knockBackTime;
            yield return new WaitForFixedUpdate();
        }
        spriteR.color = new Color(1f, 1, 1, 1f);
        pathingUnit.enablePathing();
        pathingUnit.speed = currentSpeed;
    }

    private void Death()
    {
        Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
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

    public void getAnglePath()                                    
    {
            Vector2 direction = pathingUnit.direction;
        if (direction != Vector2.zero)
        {
          float angle = Vector2.Angle(direction, new Vector2(0, -1));
            if (direction.x < 0) angle = 360 - angle;
            Angle = angle;
        }
 

    }

    public void getAngleTarget()
    {
        Vector2 direction = chaseTarget.transform.position - transform.position;
        float angle = Vector2.Angle(direction, new Vector2(0, -1));
        if (direction.x < 0) angle = 360 - angle;
        Angle = angle;
    }
    public void Slow(float slowAmount, float duration, bool fade)
    {
        if (fade)
        {
            StartCoroutine(SlowFade(slowAmount,duration));
        }
        else
        {
            StartCoroutine(SlowNonFade(slowAmount,duration));
        }

    }
    IEnumerator SlowNonFade(float slowAmount ,float duration)
    {
        float time = 0;

        currentSpeed *= (1 - slowAmount);
        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        currentSpeed /= (1 - slowAmount);
    }
    IEnumerator SlowFade(float slowAmount, float duration)
    {
        float speed = 1f;
        float time = 0;
        while (time < duration)
        {
            currentSpeed /= speed;
            speed = (time / duration) * slowAmount + 1 - slowAmount;
            currentSpeed *= speed;

            time += Time.deltaTime;
            yield return null;
        }
        currentSpeed /= speed;
    }
    public void Root(float time)
    {
        isRooted = true;
        Invoke("UnRoot", time);
    }
    public void UnRoot()
    {
        isRooted = false;
    }
}
