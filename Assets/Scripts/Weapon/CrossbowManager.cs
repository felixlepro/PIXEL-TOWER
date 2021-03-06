﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowManager : WeaponManager
{
    public float boltSpeed;


    public GameObject bolt;
    //private List<Bolt> boltList = new List<Bolt>();

    public FloatRange boltSpeedRange = new FloatRange(15, 20);

    public override void WeaponSetStats()
    {
        SetRarity();
        int lvl = GameManager.instance.GetCurrentLevel();
        float AdAsRation = Random.value;
        float lvlScale = Mathf.Pow(2, (float)(lvl - 1) / lvlScalability);
        //float lvlScale = 1 + (float)lvl / lvlScalability;
        attackDamage = Mathf.RoundToInt(attackDamageRange.Set(AdAsRation) * thisRarity.multiplier * lvlScale);
        attackSpeed = attackSpeedRange.Set(AdAsRation);
        attackDamageChargedBonus = attackDamageChargedBonusRange.Random * thisRarity.multiplier;
        knockBackAmount = knockBackAmountRange.Set(1 - AdAsRation) * thisRarity.multiplier;

        float boltSpeedSlowAmountRatio = Random.value;
        boltSpeed = boltSpeedRange.Set(boltSpeedSlowAmountRatio) * thisRarity.multiplier;

        cost = Mathf.RoundToInt((costRange.Random + (lvl - 1) * costScalability) * thisRarity.multiplier);

        float slowDurationValueRatio = Random.value;
        float burnDurationValueRatio = Random.value;
        burnDuration = burnDurationRange.Set(burnDurationValueRatio);
        burnSuffered = burnSufferedRange.Set(1 - burnDurationValueRatio);
        chanceBurnProc = chanceBurnProcRange.Random;
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

    protected override void ChargeWeapon()
    {
        crossBowAttack();
    }

    protected override void MaxChargeWeapon()
    {
        crossBowAttack();
    }
    
    protected override void ReleaseChargedWeapon()
    {
        crossBowAttack();
    }

    void crossBowAttack()
    {
        anim.SetTrigger("isFireing");
     
        if (!isFantoming)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            direction = new Vector3(mousePosition.x - transform.parent.transform.parent.position.x, mousePosition.y - transform.parent.transform.parent.transform.position.y - 0.5f, 0f);
        }
        Instantiate(bolt, transform.position, Quaternion.identity).GetComponent<Bolt>().Setup(direction, boltSpeed, boltSpeed, GetComponent<WeaponManager>());//(attackDamage, direction, knockBackAmount, boltSpeed, boltSpeed, chanceBurnProc, burnSuffered, burnDuration, chanceSlowProc, slowValue, slowDuration); ;

        // boltList[boltList.Count - 1]
        ResetAttackTimer();
    }
    protected override SpriteRenderer GetSpriteRenderer()
    {
        return GetComponent<SpriteRenderer>();
    }
    protected override void WeaponOnCD()
    {
        
    }
}
