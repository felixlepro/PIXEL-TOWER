using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManager : WeaponManager {
    public float boltSpeed;
    public float slowAmount;

    public GameObject bolt;
    private List<Bolt> boltList = new List<Bolt>();

    public FloatRange boltSpeedRange = new FloatRange(20, 30);
    public FloatRange slowAmountRange = new FloatRange(0.25f, 0.75f);


    public override void WeaponSetStats()
    {
        SetRarity();

        float AdAsRation = Random.value;

        attackDamage = Mathf.RoundToInt(attackDamageRange.Set(AdAsRation) * rarity);
        attackSpeed = attackSpeedRange.Set(1 - AdAsRation) * rarity;
        attackDamageChargedBonus = attackDamageChargedBonusRange.Random * rarity;
        knockBackAmount = knockBackAmountRange.Set(1 - AdAsRation) * rarity;

        float boltSpeedSlowAmountRatio = Random.value;
        boltSpeed = boltSpeedRange.Set(boltSpeedSlowAmountRatio) * rarity;
        slowAmount = slowAmountRange.Set(1 - boltSpeedSlowAmountRatio) / rarity;

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
       
        anim.SetBool("AttackCharge", true);
        player.currentSpeed /= (1 - (slowAmount * (1 - (1 - chargeDoneRatio) * (1 - chargeDoneRatio))));
        chargeDoneRatio = (currentChargeTime / chargeTime);
        player.currentSpeed *= (1 - (slowAmount * (1-(1-chargeDoneRatio)*(1-chargeDoneRatio))));

    }

    protected override void MaxChargeWeapon()
    {
    }

    protected override void ReleaseChargedWeapon()
    {
        player.currentSpeed /= (1 - (slowAmount * (1 - (1 - chargeDoneRatio) * (1 - chargeDoneRatio))));

        anim.SetBool("AttackCharge", false);
        anim.SetTrigger("PlayerAttack");
        boltList.Add(Instantiate(bolt, transform.position, Quaternion.identity).GetComponent<Bolt>());

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0f);

        boltList[boltList.Count - 1].Setup(attackDamage, direction, knockBackAmount, boltSpeed * chargeDoneRatio, boltSpeed);
        ResetAttackTimer();
    }

    protected override void WeaponOnCD()
    {
    }
}

