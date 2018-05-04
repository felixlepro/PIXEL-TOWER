using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;

abstract public class EnemyManager : Character {

    public float patrolSpeedChaseSpeedRatio;
    public float gettingKnockedBackAmount;
    //public float attackRange;
    public float chaseRange;
    public float chaseRangeBuffer;
    public int fireStack = 0;
    public float attackSpeed;
    public int nbrCoins;
    public int weaponDropChance;
    public Image hpBar;
    [HideInInspector]  public Attacks[] attacks;
    protected const float lvlScalability = 6f; //after how many floors will the stats double 
    protected float lvlScaleEx;
   // public GameObject[] attacksUPF; //attackUsingPrefabs
   //public struct Atta 
   //{
   //    public int attackDamage;
   //    public float immuneTime;
   //    public float maxKnockBackAmount;
   //    public float attackRange;
   //    public GameObject prefab;
   //    [HideInInspector] public Collider2D[] attackHitbox;
   //}

    public AudioClip dun;
    public GameObject dunExlamation;
    public float height;

    [HideInInspector] public bool isRooted = false;
    [HideInInspector] public float timeUntilNextAttack;
    [HideInInspector] public Rigidbody2D enemyRigidbody;
    [HideInInspector] public Animator anim;
    protected bool updateAnim = true;
     public StateController controller;
    [HideInInspector] public SpriteRenderer spriteR;
    [HideInInspector] public Collider2D targetCollider;
    Collider2D[]  enemyCollider;
    [HideInInspector] public float Angle;

    [HideInInspector] public float knockBackAmount = 0;
    [HideInInspector] const float knockBackAmountOverTimeMinimum = 0.85f;
    [HideInInspector] public Vector2 knockBackDirection;
    [HideInInspector] public Color couleurKb = Color.white;
    const float timePerKnockBackAmount = 10; //10 kba lasts 1 seconds
    bool onlyOneAttack = false;
    [HideInInspector] public bool gotDamaged = false;
   
    public Transform chaseTarget;
    bool dead = false;
    [HideInInspector] public Unit pathingUnit;

   
    //public int attackDamage;
    //public float attackRange;
    //public float maxKnockBackAmount;
    //public float immuneTime;

    public float idleTime;


    [HideInInspector]   public List<Vector3> wayPointList;
    [HideInInspector] public int nextWayPoint = 0;

    abstract protected void OnStart();
    abstract public bool CheckAttack();
    abstract public void setAnimState(string newState);
    abstract public string getAnimState();
    abstract public void TryAttack();
    abstract public void Damaged();
    abstract public void AttackSuccessful();
    abstract public void UpdateAnim();
    abstract public void gonnaDie();
    abstract public void SpecificStats(int lvl);

    private void OnEnable()
    {
        controller = GetComponent<StateController>();

    }

    void Start()
    {
        attacks = GetComponents<Attacks>();
        if (attacks.Length == 1)
        {
            onlyOneAttack = true;
        }
        hp = maxHp;
        anim = GetComponentInChildren<Animator>();
        currentSpeed = maxMoveSpeed/patrolSpeedChaseSpeedRatio;
        pathingUnit = GetComponent<Unit>();
        pathingUnit.speed = currentSpeed;
       // pathingUnit.enabled = false;

        chaseTarget = GameManager.instance.player.transform;
        

        enemyRigidbody = GetComponent<Rigidbody2D>();

        spriteR = gameObject.transform.Find("EnemyGraphics").gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.color = wColor;

        enemyCollider = GetComponentsInChildren<Collider2D>();
        targetCollider = chaseTarget.GetComponent<Collider2D>();
        canvas = GameObject.Find("Canvas");
        OnStart();
    }
    virtual public void SetStats(int lvl)
    {
        lvlScaleEx = Mathf.Pow(2, (float)(lvl-1) / lvlScalability);
        //float lvlScale = 1 + (float)lvl / lvlScalability;
        maxHp = Mathf.RoundToInt(maxHp * lvlScaleEx);
        hp = maxHp;
        SpecificStats(lvl);
    }
    private void Update()
    {
        if (isRooted)     pathingUnit.speed = 0;    
        else               pathingUnit.speed = currentSpeed;
        UpdateAnim();
        UpdatecurrentAttackCD();
    }

    public void SetupAI( List<Vector3> wayPointsFromGameManager)
    {
       
        wayPointList.Clear();
        wayPointList = wayPointsFromGameManager;
        nextWayPoint = Random.Range(0, wayPointList.Count);

     //   Debug.Log("wpl  " + wayPointList.Count);
    }
    public void ActivateAI(bool tf)
    {
        controller.aiActive = tf;
        GetComponent<Unit>().enabled = true;
    }
    public void Attack(Attacks at)
    {
        if (!targetCollider.gameObject.GetComponent<Player>().immune)
        {
           // foreach (Collider2D pc in targetCollider)
            {
                foreach (Collider2D ah in at.attackHitbox)
                {
                    if (ah.IsTouching(targetCollider))
                    {
                        targetCollider.gameObject.GetComponent<Player>().RecevoirDegats(at.attackDamage, targetCollider.gameObject.transform.position - transform.position, at.maxKnockBackAmount, at.immuneTime);
                        targetCollider.gameObject.GetComponent<Player>().Burn(at.burnChance, at.burnDamage, at.burnDuration);
                        targetCollider.gameObject.GetComponent<Player>().Slow(at.slowChance , at.slowAmount, at.slowDuration,false);
                        AttackSuccessful();
                        //resetAttackCD();
                        break;
                    }
                }
            }
        }
    }
    public void idling()
    {
        float time = Random.Range(1, 5) * idleTime;
        setAnimState("Idling");
        pathingUnit.disablePathing();
        //newPath();
        Invoke("newPath", time);    
    }
    private void newPath()
    {
        setAnimState("Moving");
        nextWayPoint = Random.Range(0, wayPointList.Count-1);

        pathingUnit.targetPosition = wayPointList[nextWayPoint];        
        pathingUnit.enablePathing(true);

    }


    //public void checkDistanceToPlayer()
    //{
    //    if ((controller.chaseTarget.transform.position - transform.position).magnitude <= enemy.size)
    //    {
    //        //controller.AIPathing.reachedEndOfPath = true;
    //    }

    //}

    public override void RecevoirDegats(int damage, Vector3 kbDirection, float kbAmmount, float im)
    {
        if (!immune)
        {
            hp -= damage;
            // Debug.Log(hp);
            hpBar.fillAmount = (float)hp / (float)maxHp;
            //  Debug.Log(hpBar.fillAmount);
            DamageTextManager.CreateFloatingText(damage, transform.position);
            CameraShaker.Instance.ShakeOnce(damage/lvlScaleEx * 0.15f, 2.5f, 0.1f, 0.7f);
            float kbTemp = kbAmmount * gettingKnockedBackAmount;
            Damaged();
            gotDamaged = true;
            if (kbTemp != 0)
            {
                knockBackAmount = kbTemp;
                knockBackDirection = kbDirection.normalized;
                //knockBackAmountOverTime = 0;
                StartCoroutine("KnockBack");
            }
            else StartCoroutine("RedOnly");

            VerifyDeath();
        }
        
    }
    public void VerifyDeath()
    {
        if (hp <= 0 && !dead)
        {
            dead = true;
            foreach(Collider2D cl in enemyCollider)
            {
                cl.enabled = false;
            }
            controller.aiActive = false;
            gonnaDie();
            spriteR.color = Color.white;
            StopAllCoroutines();
            isRooted = true;
            currentSpeed = maxMoveSpeed;
            setAnimState("Dying");
            UpdateAnim();
            updateAnim = false;
            DropItems();
            Destroy(this.gameObject , 1);
            //   Invoke("Death", 2);
        }
     
    }
    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    Debug.Log("triggered");
    //    if (other.tag == "Obstacle" && !other.isTrigger)
    //    {
    //        Debug.Log("triggered");
    //        hitAWall = true;
    //    }
    //}
    //void OnTriggerExit2D(Collider2D other)
    //{
    //    hitAWall = false;
    //}

    public IEnumerator RedOnly()
    {
        float kbAmountOverTime = 0;
        spriteR.color = new Color(1f, 0, 0, 1f);
        while (kbAmountOverTime < knockBackAmountOverTimeMinimum)
        {
           // if (!hitAWall)
            {
                float curve = (1 - kbAmountOverTime) * (1 - kbAmountOverTime);
                spriteR.color = new Color(1f, 1 - curve, 1 - curve, 1f);

                kbAmountOverTime += Time.deltaTime * 1.75f;
            }
            yield return null;
        }
        spriteR.color = new Color(1f, 1, 1, 1f);
    }
    public IEnumerator KnockBack()
    {
        isRooted = true;
        pathingUnit.disablePathing();
        float kbAmountOverTime = 0;
        spriteR.color = new Color(1f, 0, 0, 1f);

        float knockBackTime = (timePerKnockBackAmount / knockBackAmount);
        while (kbAmountOverTime < knockBackAmountOverTimeMinimum)
        {
            float curve = (1 - kbAmountOverTime) * (1 - kbAmountOverTime);
            spriteR.color = new Color(1f, 1 - curve, 1 - curve, 1f);

           

            Vector3 kb = knockBackDirection * knockBackAmount * curve * Time.deltaTime;
            //Debug.Log(kb + "    " + knockBackAmount + "    " + curve);
            enemyRigidbody.MovePosition(transform.position + kb);
            //transform.position = Vector3.MoveTowards(transform.position, transform.position+kb, Time.deltaTime);
            kbAmountOverTime += Time.deltaTime * knockBackTime;
            yield return new WaitForFixedUpdate();
        }
        spriteR.color = new Color(1f, 1, 1, 1f);
        pathingUnit.enablePathing(true);
        isRooted = false;
    }

    //private void Death()
    //{

    //    Destroy(this.gameObject);
    //    //this.gameObject.SetActive(false);
    //}

    public void UpdatecurrentAttackCD()
    {
        if (timeUntilNextAttack > 0)
        {
            timeUntilNextAttack -= Time.deltaTime;
        }
        if(!onlyOneAttack)
        {
            foreach (Attacks at in attacks)
            {
                at.UpdatecurrentAttackCD();
            }
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
    public void Slow(float slowAmount, float duration, bool  fade)
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
    public IEnumerator Root(float time)
    {
        isRooted = true;
        setAnimState("Idling");
       yield return new WaitForSeconds(time);
        isRooted = false;
        setAnimState("Moving");
    }
//    public void Root(float time)
//    {
//        isRooted = true;
//        Invoke("UnRoot", time);
//        setAnimState("Idling");
//}
//    public void UnRoot()
//    {
//        isRooted = false;
//        setAnimState("Moving");
//    }
 

    protected Vector3 playerMovementPrediction(float castTime, float predictionAmount)
    {
        if (chaseTarget.gameObject == GameManager.instance.player)
        {
            return chaseTarget.position + chaseTarget.GetComponent<Player>().movement.normalized * chaseTarget.GetComponent<Player>().currentSpeed * castTime / predictionAmount;
        }
        else
        {
            return chaseTarget.position;
        }

    }

    public void playDun()
    {
        StartCoroutine(Root(0.75f));
        GameManager.instance.PlaySound(dun);
        GameObject dungo = Instantiate(dunExlamation, transform.position + Vector3.up*height, Quaternion.identity);
        dungo.GetComponentInChildren<DunManager>().Initialize(transform.position + Vector3.up * height);

    }
    protected virtual void DropItems()
    {
        nbrCoins = Random.Range(Mathf.RoundToInt(nbrCoins / 2), Mathf.RoundToInt(nbrCoins * 1.5f));
        DropManager.DropCoin(transform.position, nbrCoins);
        if (Random.value *100 < weaponDropChance)
        {
            DropManager.DropRandomWeapon(transform.position + Vector3.up/2,true);
        }
    }

    public override Vector3 PositionIcone()
    {
        return transform.position;
    }
}