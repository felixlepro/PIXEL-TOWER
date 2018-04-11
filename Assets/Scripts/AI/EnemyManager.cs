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
    public Image hpBar;
    [HideInInspector]  public Attacks[] attacks;
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
    [HideInInspector] public Collider2D[] targetCollider;
    Collider2D[]  enemyCollider;
    [HideInInspector] public float Angle;

    [HideInInspector] public float knockBackAmount = 0;
    [HideInInspector] const float knockBackAmountOverTimeMinimum = 0.85f;
    [HideInInspector] public Vector2 knockBackDirection;
    [HideInInspector] public Color couleurKb = Color.white;
    const float timePerKnockBackAmount = 10; //10 kba lasts 1 seconds
    bool onlyOneAttack = false;

    public Transform chaseTarget;

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
        anim = GetComponentInChildren<Animator>();
        currentSpeed = maxMoveSpeed/patrolSpeedChaseSpeedRatio;
        pathingUnit = GetComponent<Unit>();
        pathingUnit.speed = currentSpeed;

        chaseTarget = GetComponentInParent<PlayerTarget>().playerTarget;
        hp = maxHp;

        enemyRigidbody = GetComponent<Rigidbody2D>();

        spriteR = gameObject.transform.Find("EnemyGraphics").gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.color = wColor;

        enemyCollider = GetComponentsInChildren<Collider2D>();
        targetCollider = chaseTarget.GetComponents<Collider2D>();
        OnStart();
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

        wayPointList = wayPointsFromGameManager;
        controller.aiActive = true;
        nextWayPoint = Random.Range(0, wayPointList.Count);
    }
    public void Attack(Attacks at)
    {
        if (!targetCollider[0].gameObject.GetComponent<Player>().immune)
        {
            foreach (Collider2D pc in targetCollider)
            {
                foreach (Collider2D ah in at.attackHitbox)
                {
                    if (ah.IsTouching(pc))
                    {
                        pc.gameObject.GetComponent<Player>().RecevoirDegats(at.attackDamage, pc.gameObject.transform.position - transform.position, at.maxKnockBackAmount, at.immuneTime);
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
        pathingUnit.enablePathing();

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
        hp -= damage;
       // Debug.Log(hp);
        hpBar.fillAmount = (float)hp / (float)maxHp;
      //  Debug.Log(hpBar.fillAmount);
        DamageTextManager.CreateFloatingText(damage, transform.position);
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
            Destroy(this.gameObject , 2);
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
        yield return new WaitForFixedUpdate();
        pathingUnit.speed = 0;
        pathingUnit.disablePathing();
        float kbAmountOverTime = 0;
        spriteR.color = new Color(1f, 0, 0, 1f);

        float knockBackTime = (timePerKnockBackAmount / knockBackAmount);
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
        return chaseTarget.position + chaseTarget.GetComponent<Player>().movement.normalized * chaseTarget.GetComponent<Player>().currentSpeed * castTime/predictionAmount;

    }

    public void playDun()
    {
        StartCoroutine(Root(0.75f));
        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySound(dun);
        GameObject dungo = Instantiate(dunExlamation, transform.position + Vector3.up*height, Quaternion.identity);
        dungo.GetComponentInChildren<DunManager>().Initialize(transform.position + Vector3.up * height);

    }

}