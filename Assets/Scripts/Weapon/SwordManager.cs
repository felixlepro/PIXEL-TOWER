using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : WeaponManager
{
    private BoxCollider2D coll;
    Animator[] anima;
    float thisAttackCCT = 0;



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

        float AdAsRation = Random.value;

        attackDamage = Mathf.RoundToInt(attackDamageRange.Set(AdAsRation) * rarity);
        attackSpeed = attackSpeedRange.Set(1-AdAsRation) * rarity;
        attackDamageChargedBonus = attackDamageChargedBonusRange.Random * rarity;
        knockBackAmount = knockBackAmountRange.Set(1 - AdAsRation) * rarity;

        cost = 10;

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
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Colision");
        if (other.tag == "Enemy")
        {
            
            //Debug.Log("En");
            //EnemyManager enemyManager = other.gameObject.GetComponentInParent<EnemyManager>();
            chargeDoneRatio = thisAttackCCT;
            if (coll.enabled)
            {
                EnvoyerDegat(other.gameObject.GetComponentInParent<EnemyManager>());
            }
            coll.enabled = false;
            chargeDoneRatio = 0;
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
        currentChargeTime = 0;
        coll.enabled = false;
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
    
    void attack()
    {
       anima[0].speed = 1;
        anima[0].SetBool("AttackCharge", false);
        //anima[0].SetBool("AttackChargeMax", false);
      //  anima[0].SetTrigger("PlayerAttack");
        Invoke("triggerSwipe", 0.1f);
        ResetAttackTimer();  
        GetComponentInParent<Player>().doFaceMouse(false);
        Invoke("facingMouse", anima[0].GetCurrentAnimatorStateInfo(0).length * anima[0].GetCurrentAnimatorStateInfo(0).speed * 0.7f);
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
}
