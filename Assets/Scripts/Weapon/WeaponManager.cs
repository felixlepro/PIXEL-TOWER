using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponManager : MonoBehaviour {

    public GameObject weaponPrefab;
    public string weaponName;
    public Color wColor;
    public int attackDamage;
    public int cost;
   // public float range;
    public float attackSpeed; //  attackCD
    //Attributs responsables des effets de Burn et de Slow (propre à chaque arme)
    public bool isFire = false;
    public bool isIce = false;

    [HideInInspector]public int chanceBurnProc = 30;
    [HideInInspector]public int chanceSlowProc = 40;

    [HideInInspector] public float burnDuration = 4;
    [HideInInspector] public int burnSuffered = 5;

    [HideInInspector] public int slowDuration = 3;
    [HideInInspector] public float slowValue = 0.3f;

   [HideInInspector]public bool slowFadeState = false;     
    //Fin des attributs d'effets spéciaux d'armes  -Simon


    public float chargeTime;
    public int attackDamageChargedBonus;
    public float knockBackAmount;
    [HideInInspector] public RuntimeAnimatorController animator;
    public Sprite sprite;
    public string description;
    public Vector3 basePosition = new Vector3(0.35f, 0, 0);
    public Vector3 baseScale = new Vector3(1, 1, 1);
    public int numAttack;
    public bool isFantoming = false;
    protected  SpriteRenderer spriteR;
    protected Animator anim;


    protected float rand;
    protected Player player;
    protected float chargeDoneRatio;
    protected  float timeUntilNextAttack;
    protected float time;
    protected float currentChargeTime;
     KeyCode chargeAttackKey = KeyCode.Mouse0;

    float randomR;

    float rarity;

    const float common = 60;

    const float rare = 25;

    const float epic = 15;

    const float legendary = 4.99f;

    const float ultraLegendary = 0.01f;

    protected abstract void ChargeWeapon();
    protected abstract void MaxChargeWeapon();
    protected abstract void ReleaseChargedWeapon();
    protected abstract void WeaponOnCD();
    public abstract void WeaponSetStats();

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
        if (!isFantoming) setNumAttack();
        Attack(numAttack);
    }
    public void setNumAttack()
    {
        if (Input.GetKey(chargeAttackKey)) numAttack = 1;
        else if (Input.GetKeyUp(chargeAttackKey)) numAttack = 2;
        else numAttack = 0;
        
    }
    public void Attack(int attack)
    {
        if (timeUntilNextAttack <= 0)
        {

            if (numAttack == 1 && (currentChargeTime < chargeTime))
            {

                currentChargeTime += Time.deltaTime;
                ChargeWeapon();
            }
            else if (numAttack == 1 && (currentChargeTime >= chargeTime))
            {
                MaxChargeWeapon();
            }
            else if (numAttack == 2)
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

        if (isFire)
        {
            if (NbRand(0,100) < chanceBurnProc)
            {
                cible.Burn(burnDuration,burnSuffered);
            }
        }
        else if (isIce)
        {
            if (NbRand(0,100) < chanceSlowProc)
            {
                cible.Slow(slowValue, slowDuration, slowFadeState);
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

    protected int NbRand(int min, int max)
    {
        rand = Random.Range(min,max);
        return Mathf.FloorToInt(rand);
    }
    
    public void SetRarity()
    {
        randomR = NbRand(0,100);
       
    }
    public void WeaponSetEffect(bool Fire,bool frozen)
    {
        if(Fire)
        {
            isFire = true;
        }
        if (frozen)
        {
            isIce = true;
        }
    }

}

    
   

