using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearManager : WeaponManager
{
    private BoxCollider2D coll;
    List<GameObject> enemyAlreadyHit = new List<GameObject>();
    float thisAttackCCT = 0;
    const float attackTime = 0.25f;

    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = wColor;

        coll = gameObject.GetComponent<BoxCollider2D>();
        coll.enabled = false;
       
    }

    public override void WeaponSetStats()
    {
        SetRarity();
        int lvl = GameManager.instance.GetCurrentLevel();
        float AdAsRation = Random.value;
        float lvlScale = Mathf.Pow(2, (float)(lvl - 1) / lvlScalability);
        attackDamage = Mathf.RoundToInt(attackDamageRange.Set(AdAsRation) * thisRarity.multiplier * lvlScale);
        attackSpeed = attackSpeedRange.Set(AdAsRation) + attackTime;
        attackDamageChargedBonus = attackDamageChargedBonusRange.Random * thisRarity.multiplier;
        knockBackAmount = knockBackAmountRange.Set(1 - AdAsRation) * ((thisRarity.multiplier - 1) / 2 + 1);

        cost = Mathf.RoundToInt((costRange.Random + (lvl - 1) * costScalability) * thisRarity.multiplier);

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
        if (other.tag == "Enemy" && transform.parent != null)
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
        else if (other.tag == "Chest")
        {
            other.gameObject.GetComponent<Chest>().OpenChest();
            other.tag = "OpenChest";
        }
    }

    protected override void ChargeWeapon()
    {
        chargeDoneRatio = (currentChargeTime / chargeTime);
        anim.SetBool("AttackCharge", true);
    }
    protected override void MaxChargeWeapon()
    {
    }
    protected override void ReleaseChargedWeapon()
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
        anim.SetBool("AttackCharge", false);
        ResetAttackTimer();
        Invoke("EndAttack", attackTime);
        if(!isFantoming) GetComponentInParent<Player>().doFaceMouse(false);
        Invoke("facingMouse", attackTime);
    }

    public override bool CanSwitch()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
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
    private void OnDisable()
    {
        anim.enabled = false;
    }
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.enabled = true;
    }
    protected override SpriteRenderer GetSpriteRenderer()
    {
        return GetComponent<SpriteRenderer>();
    }
}