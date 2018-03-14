﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager: WeaponManager 
{
    private BoxCollider2D coll;


    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = wColor;

        coll = gameObject.GetComponent<BoxCollider2D>();
        coll.enabled = false;

        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = animator;     
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Colision");
        if (other.tag == "Enemy")
        {
            //Debug.Log("En");
            EnemyManager enemyManager = other.gameObject.GetComponentInParent<EnemyManager>();
            EnvoyerDegat(enemyManager);
        }
        else if (other.tag== "Chest")
        {
            other.gameObject.GetComponent<Chest>().OpenChest();
        }
    }

    protected override void ChargeWeapon()
    {
        chargeDoneRatio = (currentChargeTime / chargeTime);
        anim.speed = 1 + (chargeDoneRatio * chargeDoneRatio * 1.5f);
        anim.SetBool("AttackCharge", true);
    }
    protected override  void MaxChargeWeapon()
    {
        anim.SetBool("AttackCharge", true);
        anim.speed = 2.5f;
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
        anim.speed = 1;
        anim.SetBool("AttackCharge", false);
        anim.SetBool("AttackChargeMax", false);
        anim.SetTrigger("PlayerAttack");
        ResetAttackTimer();
        currentChargeTime = 0;
        GetComponentInParent<Player>().doFaceMouse(false);
        Invoke("facingMouse", anim.GetCurrentAnimatorStateInfo(0).length * anim.GetCurrentAnimatorStateInfo(0).speed);
    }
}
