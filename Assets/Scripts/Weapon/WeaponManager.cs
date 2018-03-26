using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponManager : MonoBehaviour {

    public GameObject weaponPrefab;
    public string weaponName;
    public Color wColor;
    public int attackDamage;
    public int cost;
    public float range;
    public float attackSpeed; //  attackCD
    public bool IsFire = false;
    public bool IsIce = false;

    public int chanceBurnProc = 30;
    public int chanceSlowProc = 40;
    public float chargeTime;
    public int attackDamageChargedBonus;
    public float knockBackAmount;
    public RuntimeAnimatorController animator;
    public Sprite sprite;
    public string description;
    public Vector3 basePosition = new Vector3(0.35f, 0, 0);
    public Vector3 baseScale = new Vector3(1, 1, 1);
    protected  SpriteRenderer spriteR;
    protected Animator anim;

    protected float rand;
    protected Player player;
    protected float chargeDoneRatio;
    protected  float timeUntilNextAttack;
    protected float time;
    protected float currentChargeTime;
     KeyCode chargeAttackKey = KeyCode.Mouse0;

    protected abstract void ChargeWeapon();
    protected abstract void MaxChargeWeapon();
    protected abstract void ReleaseChargedWeapon();
    protected abstract void WeaponOnCD();

    void Start()
    {
        player = GetComponentInParent<Player>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = wColor;
       
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = animator;
    }

    void Update()
    {
        UpdateTimeUntilNextAttack();
        if (timeUntilNextAttack <= 0)
        {

            if (Input.GetKey(chargeAttackKey) && (currentChargeTime < chargeTime))
            {
                
                currentChargeTime += Time.deltaTime;
                ChargeWeapon();               
            }
            else if (Input.GetKey(chargeAttackKey) && (currentChargeTime >= chargeTime))
            {
                MaxChargeWeapon();      
            }
            else if (Input.GetKeyUp(chargeAttackKey))
            {
                ReleaseChargedWeapon();
                currentChargeTime = 0;
                chargeDoneRatio = 0;
            }

        }
        else
        {
            WeaponOnCD();
        }
    }


    public void EnvoyerDegat(EnemyManager cible)
    {
        if (currentChargeTime < chargeTime)
        {
            cible.recevoirDegats(attackDamage + Mathf.FloorToInt(attackDamage + attackDamageChargedBonus * chargeDoneRatio * chargeDoneRatio), cible.gameObject.transform.position - transform.position, knockBackAmount);
        }
        else
        {
            cible.recevoirDegats(attackDamageChargedBonus + attackDamage, cible.gameObject.transform.position - transform.position, knockBackAmount);
        }

        if (IsFire)
        {
            if (NbRand() < chanceBurnProc)
            {
                cible.Burn();
            }
        }
        if (IsIce)
        {
            if (NbRand() < chanceSlowProc)
            {
                 //cible.Slow();
            }          
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
        timeUntilNextAttack = attackSpeed;
    }

    protected int NbRand()
    {
        rand = Random.value;
        return Mathf.FloorToInt(rand);
    }
}
