using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponManager : MonoBehaviour {

    [HideInInspector] public Weapon weapon;
    protected  SpriteRenderer spriteR;
    protected Animator anim;

    protected float chargeDoneRatio;
    protected  float timeUntilNextAttack;
    protected float time;
    protected float currentChargeTime;
    public KeyCode chargeAttackKey;

    protected abstract void ChargeWeapon();
    protected abstract void MaxChargeWeapon();
    protected abstract void ReleaseChargedWeapon();
    protected abstract void WeaponOnCD();

    void Start()
    {
        weapon = GetComponentInParent<Player>().player.weapon;
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = weapon.wColor;
       
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = weapon.animator;
    }

    void Update()
    {
        UpdateTimeUntilNextAttack();

        if (timeUntilNextAttack <= 0)
        {

            if (Input.GetKey(chargeAttackKey) && (currentChargeTime < weapon.chargeTime))
            {
                currentChargeTime += Time.deltaTime;
                ChargeWeapon();

            }
            else if (Input.GetKey(chargeAttackKey) && (currentChargeTime >= weapon.chargeTime))
            {
                MaxChargeWeapon();
            }
            else if (Input.GetKeyUp(chargeAttackKey))
            {
                ReleaseChargedWeapon();
            }

        }
        else
        {
            WeaponOnCD();
        }
    }

    public void EnvoyerDegat(EnemyManager cible)
    {
        if (currentChargeTime < weapon.chargeTime)
        {
            cible.recevoirDegats(weapon.attackDamage + Mathf.FloorToInt(weapon.attackDamageChargedBonus * chargeDoneRatio * chargeDoneRatio), cible.gameObject.transform.position - transform.position, weapon.knockBackAmount);
        }

        else
        {
            cible.recevoirDegats(weapon.attackDamageChargedBonus + weapon.attackDamage, cible.gameObject.transform.position - transform.position, weapon.knockBackAmount);
        }
    }

    protected void facingMouse()
    {
        GetComponentInParent<Player>().doFaceMouse(true);
    }
    protected void UpdateTimeUntilNextAttack()
    {
        if (timeUntilNextAttack > 0)
        {
            timeUntilNextAttack -= Time.deltaTime;
        }
    }
     protected void ResetAttackTimer()
    {
        timeUntilNextAttack = weapon.attackSpeed;
    }

}
