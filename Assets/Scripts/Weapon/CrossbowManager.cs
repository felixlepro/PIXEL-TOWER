﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowManager : WeaponManager
{
    public float boltSpeed;


    public GameObject bolt;
    private List<Bolt> boltList = new List<Bolt>();

    public FloatRange boltSpeedRange = new FloatRange(15, 20);

    public override void WeaponSetStats(int lvl)
    {
        SetRarity();

        float AdAsRation = Random.value;
        float lvlScale = 1 + (float)lvl / lvlScalability;
        attackDamage = Mathf.RoundToInt(attackDamageRange.Set(AdAsRation) * rarity * lvlScale);
        attackSpeed = attackSpeedRange.Set(1 - AdAsRation) * rarity;
        attackDamageChargedBonus = attackDamageChargedBonusRange.Random * rarity;
        knockBackAmount = knockBackAmountRange.Set(1 - AdAsRation) * rarity;

        float boltSpeedSlowAmountRatio = Random.value;
        boltSpeed = boltSpeedRange.Set(boltSpeedSlowAmountRatio) * rarity;

        cost = 10;

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
        
    }

    protected override void MaxChargeWeapon()
    {
        
    }

    protected override void ReleaseChargedWeapon()
    {
        anim.SetTrigger("isFireing");
        boltList.Add(Instantiate(bolt, transform.position, Quaternion.identity).GetComponent<Bolt>());

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0f);

        boltList[boltList.Count - 1].Setup(attackDamage, direction, knockBackAmount, boltSpeed, boltSpeed);
        ResetAttackTimer();
    }

    protected override void WeaponOnCD()
    {
        
    }
}
