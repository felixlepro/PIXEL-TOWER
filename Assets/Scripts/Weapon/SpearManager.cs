using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearManager : WeaponManager
{
    private BoxCollider2D coll;
    Animator anim;

    void Start()
    {
        //spriteR = gameObject.GetComponentInChildren<SpriteRenderer>();
        //spriteR.color = wColor;

        coll = gameObject.GetComponent<BoxCollider2D>();
        coll.enabled = false;

        anim = GetComponent<Animator>();
        //anima[].runtimeAnimatorController = animator;     
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
        else if (other.tag == "Chest")
        {
            other.gameObject.GetComponent<Chest>().OpenChest();
            other.tag = "OpenChest";
        }
    }

    protected override void ChargeWeapon()
    {
        chargeDoneRatio = (currentChargeTime / chargeTime);
        //anima[0].speed = 1 + (chargeDoneRatio * chargeDoneRatio * 1.5f);
       // anima[0].SetBool("AttackCharge", true);
    }
    protected override void MaxChargeWeapon()
    {
        //anima[0].SetBool("AttackCharge", true);
        //anima[0].speed = 2.5f;
    }
    protected override void ReleaseChargedWeapon()
    {
        coll.enabled = true;
        Debug.Log("release");
        attack();
    }
    protected override void WeaponOnCD()
    {
        coll.enabled = false;
    }
  
    void attack()
    {
        //anima[0].speed = 1;
        //anima[0].SetBool("AttackCharge", false);
        //anima[0].SetBool("AttackChargeMax", false);
        anim.SetTrigger("IsAttacking");
        //Invoke("triggerSwipe", 0.1f);
        ResetAttackTimer();
        currentChargeTime = 0;
        GetComponentInParent<Player>().doFaceMouse(false);
        Invoke("facingMouse", anim.GetCurrentAnimatorStateInfo(0).length * anim.GetCurrentAnimatorStateInfo(0).speed);
    }
    void triggerSwipe()
    {
        //anima[1].SetTrigger("Swipe");
    }

    public override void WeaponSetStats()
    {
        //throw new System.NotImplementedException();
    }
}
