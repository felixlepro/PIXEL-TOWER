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
   

//Attributs responsables des effets de Burn et de Slow ainsi que la rareté (propre à chaque arme)
    public int chanceBurnProc;
    public int chanceSlowProc;

    public int burnDuration;
    public int burnSuffered;

    public int slowDuration;
    public float slowValue;

    public bool slowFadeState;

    public bool isFire;
    public bool isIce;
    //rareté
    public float randomR;
    public float rarity;
    public float common = 60;
    public float rare = 25;
    public float epic = 15;
    public float legendary = 4.99f;
    public float ultraLegendary = 0.01f;
 //Fin des attributs d'effets spéciaux d'armes  -Simon

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
 //Vérification de la présence ou non d'un effet spéciale sur l'arme et appel de la fonction appropriée dans EnemyManager si le joueur à un nombre aléatoire qui respecte la condition d'activation.
        if (isFire)
        {
            if (NbRand() < chanceBurnProc)
            {
                cible.Burn(burnDuration,burnSuffered);
            }
        }
        if (isIce)
        {
            if (NbRand() < chanceSlowProc)
            {
                 cible.Slow(slowValue,slowDuration,slowFadeState);
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
//Génère un nombre random
    protected int NbRand()
    {
        rand = Random.value*100;
        return Mathf.FloorToInt(rand);
    }
    //Mettre random avec float de legendary
    public void WeaponSetStats()
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
    //a continuer
    public void SetRarity()
    {
        randomR = NbRand();
       
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
