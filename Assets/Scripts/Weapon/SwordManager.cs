using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : WeaponManager
{
    private BoxCollider2D coll;
    Animator[] anima;
    List<GameObject> enemyAlreadyHit = new List<GameObject>();
    float thisAttackCCT = 0;
    const float attackTime = 0.2f;

    void Start()
    {
        spriteR = gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.color = wColor;

        coll = gameObject.GetComponent<BoxCollider2D>();
        coll.enabled = false;

        anima = GetComponentsInChildren<Animator>();
        //anima[].runtimeAnimatorController = animator;     
    }

    public override void WeaponSetStats()
    {
        SetRarity();
        int lvl = GameManager.instance.GetCurrentLevel();
        Random.seed = System.DateTime.Now.Millisecond;
        float AdAsRation = Random.value;
        float lvlScale = 1 + (float)lvl / lvlScalability;
        attackDamage = Mathf.RoundToInt(attackDamageRange.Set(AdAsRation) * thisRarity.multiplier * lvlScale );
        attackSpeed = attackSpeedRange.Set(1-AdAsRation) * thisRarity.multiplier + attackTime;
        attackDamageChargedBonus = attackDamageChargedBonusRange.Random * thisRarity.multiplier;
        knockBackAmount = knockBackAmountRange.Set(1 - AdAsRation) * thisRarity.multiplier;

        cost = Mathf.RoundToInt(costRange.Random*lvlScalability*thisRarity.multiplier);

        float slowDurationValueRatio = Random.value;
        float burnDurationValueRatio = Random.value;

        chanceBurnProc = chanceBurnProcRange.Random;
        burnDuration = burnDurationRange.Set(burnDurationValueRatio);
        burnSuffered = burnSufferedRange.Set(1 - burnDurationValueRatio); 

        chanceSlowProc = chanceSlowProcRange.Random;
        slowDuration = slowDurationRange.Set(slowDurationValueRatio);
        slowValue = slowValueRanges.Set(1 - slowDurationValueRatio);

        if (Random.value < IceFireChance)
        {
            isIce = true;
        }
        else isIce = false;

        if (Random.value < IceFireChance)
        {
            isIce = isFire;
        }
        else isFire = false;

        Debug.Log(isFire + " " + isIce);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Colision");
        if (other.tag == "Enemy")
        {
            bool already = false;
            chargeDoneRatio = thisAttackCCT;
            foreach (GameObject en in enemyAlreadyHit)
            {
                if (other.gameObject == en)
                {
                    already = true;
                }
            }  
            if (!already)
            {
                EnvoyerDegat(other.gameObject.GetComponentInParent<EnemyManager>());
                enemyAlreadyHit.Add(other.gameObject);
            }
        }
        else if (other.tag== "Chest")
        {
            other.gameObject.GetComponent<Chest>().OpenChest();
            other.tag = "OpenChest";
        }
    }

    protected override void ChargeWeapon()
    {
        chargeDoneRatio = (currentChargeTime / chargeTime);
        anima[0].speed = 1 + (chargeDoneRatio * chargeDoneRatio * 1.5f);
        anima[0].SetBool("AttackCharge", true);
    }
    protected override  void MaxChargeWeapon()
    {
        //anima[0].SetBool("AttackCharge", true);
         anima[0].speed = 2.5f;
    }
    protected override  void ReleaseChargedWeapon()
    {
        thisAttackCCT = chargeDoneRatio;
        coll.enabled = true;
        attack();
    }
    protected override void WeaponOnCD()
    {
       
    }
  
    void attack()
    {
       anima[0].speed = 1;
        anima[0].SetBool("AttackCharge", false);
        //anima[0].SetBool("AttackChargeMax", false);
      //  anima[0].SetTrigger("PlayerAttack");
        Invoke("triggerSwipe", 0.1f);
        ResetAttackTimer();
        Invoke("EndAttack", attackTime);
        if (!isFantoming) GetComponentInParent<Player>().doFaceMouse(false);
        Invoke("facingMouse", attackTime*1.3f);
    }
    void triggerSwipe()
    {
        anima[1].SetTrigger("Swipe");
    }

      public override bool CanSwitch()
    {
        if (anima[0].GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            return true;
        }
        return false;
    }
    void EndAttack()
    {
        currentChargeTime = 0;
        coll.enabled = false;
        enemyAlreadyHit.Clear();
    }
    protected override SpriteRenderer GetSpriteRenderer()
    {
        return GetComponentInChildren<SpriteRenderer>();
    }
}
//void Update()
//{
//    UpdateTimeUntilNextAttack();

//    if (timeUntilNextAttack <= 0)
//    {

//        if (Input.GetKey(chargeAttackKey) && (currentChargeTime < weapon.chargeTime))
//        {
//            currentChargeTime += Time.deltaTime;
//            chargeDoneRatio = (currentChargeTime / weapon.chargeTime);
//            anim.speed = 1+ (chargeDoneRatio * chargeDoneRatio * 1.5f);
//            anim.SetBool("AttackCharge", true);

//        }
//        else if (Input.GetKey(chargeAttackKey) && (currentChargeTime >= weapon.chargeTime))
//        {

//        }
//        else if (Input.GetKeyUp(chargeAttackKey))
//        {

//        }

//    }
//    else
//    {
//        coll.enabled = false;
//    }



//}
