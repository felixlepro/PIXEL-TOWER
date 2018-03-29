using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager: WeaponManager 
{
    private BoxCollider2D coll;
    Animator[] anima;

    void Start()
    {
        spriteR = gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.color = wColor;

        coll = gameObject.GetComponent<BoxCollider2D>();
        coll.enabled = false;

        anima = GetComponentsInChildren<Animator>();
        //anima[].runtimeAnimatorController = animator;     
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Colision");
        if (other.tag == "Enemy")
        {
            //Debug.Log("En");
            //EnemyManager enemyManager = other.gameObject.GetComponentInParent<EnemyManager>();
            EnvoyerDegat(other.gameObject.GetComponentInParent<EnemyManager>());
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
        coll.enabled = true;
        attack();
    }
    protected override void WeaponOnCD()
    {
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
        anima[0].SetBool("AttackChargeMax", false);
        anima[0].SetTrigger("PlayerAttack");
        Invoke("triggerSwipe", 0.1f);
        ResetAttackTimer();
        currentChargeTime = 0;
        GetComponentInParent<Player>().doFaceMouse(false);
        Invoke("facingMouse", anima[0].GetCurrentAnimatorStateInfo(0).length * anima[0].GetCurrentAnimatorStateInfo(0).speed * 0.7f);
    }
    void triggerSwipe()
    {
        anima[1].SetTrigger("Swipe");
    }

    public override void WeaponSetStats()
    {
        isIce = false;
        isFire = false;
        chanceBurnProc = 30;
        chanceSlowProc = 40;
        burnDuration = 4;
        burnSuffered = 5;
        slowDuration = 3;
        slowValue = 0.3f;
        slowFadeState = false;
        SetRarity();
    }

}
