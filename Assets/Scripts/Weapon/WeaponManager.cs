using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponManager : MonoBehaviour {

    //public GameObject weaponPrefab;
    public string weaponName;
    public Color wColor;
    public int attackDamage;
    public float attackSpeed; //  attackCD
    public float chargeTime;
    public float attackDamageChargedBonus;
    public float knockBackAmount;
    public int cost; 
    public bool isMelee;

    protected const float lvlScalability = 6f; //apres combien de floor les stats des armes vont doubler
    protected const float costScalability = 5f; 
    //Attributs responsables des effets de Burn et de Slow (propre à chaque arme)
    public bool isFire = true;
    public bool isIce = false;

    [HideInInspector] protected int chanceBurnProc ;
    [HideInInspector] protected int chanceSlowProc ;

    [HideInInspector] protected float burnDuration ;
    [HideInInspector] protected int burnSuffered;

    [HideInInspector] protected float slowDuration;
    [HideInInspector] protected float slowValue;

    [HideInInspector] protected bool slowFadeState = true;
    //Fin des attributs d'effets spéciaux d'armes  -Simon



    [HideInInspector] public RuntimeAnimatorController animator;
    public Sprite sprite;
    public string description;
    public Vector3 basePosition = new Vector3(0.35f, 0, 0);
    public Vector3 baseScale = new Vector3(1, 1, 1);
    public Vector3 direction;

    [HideInInspector] public int numAttack;
    [HideInInspector] public bool isFantoming = false;
    protected SpriteRenderer spriteR;
    protected Animator anim;


    //protected float rand;
    protected Player player;
    protected float chargeDoneRatio;
    protected float timeUntilNextAttack;
    protected float time;
    protected float currentChargeTime;
    KeyCode chargeAttackKey = KeyCode.Mouse0;

    public IntRange costRange = new IntRange(25, 35);
    public IntRange attackDamageRange = new IntRange(10, 20);
    public FloatRange attackSpeedRange = new FloatRange(0.1f, 0.4f);
    public FloatRange attackDamageChargedBonusRange = new FloatRange(0.25f, 0.75f);
    public FloatRange knockBackAmountRange = new FloatRange(5f, 12f);

    public float IceFireChance = 0.225f;
    public IntRange chanceBurnProcRange = new IntRange(25, 75);
    public IntRange burnDurationRange = new IntRange(1, 4);
    public IntRange burnSufferedRange = new IntRange(5, 10);
    public IntRange chanceSlowProcRange = new IntRange(25, 75);
    public FloatRange slowDurationRange = new FloatRange(1, 3);
    public FloatRange slowValueRanges = new FloatRange(0.1f, 0.4f);

    public struct rarity
    {
        public float chance { get; set; }
        public float multiplier { get; set; }
        public string name { get; set; }
    }
    [HideInInspector] public rarity thisRarity;
    rarity[] possibleRarities = { new rarity() { chance = 100, multiplier = 1, name = "Commun" },
                                  new rarity() { chance = 55, multiplier = 1.15f, name = "Rare" },
                                  new rarity() { chance = 22, multiplier = 1.3f, name = "Épic" },
                                  new rarity() { chance = 6, multiplier = 1.45f, name = "Légendaire" },
                                  new rarity() { chance = 1f, multiplier = 1.6f, name = "Mythique" },
                                  new rarity() { chance = 0.2f, multiplier = 1.75f, name = "Divin" },
                                 new rarity()  { chance = 0.00001f, multiplier = 3f, name = "Paradoxal" }
    };

                                 


       // = { { 100, 1 }, { 50, 1.2f }, { 20, 1.4f }, { 5, 1.6f }, { 0.01f, 2f } };

    //const float commonChance = 100;
    //const float rareChance = 50;
    //const float epicChance = 20;
    //const float legendaryChance = 5f;
    //const float ultraLegendaryChance = 0.01f;

    //const float commonMult = 1;
    //const float rareMult = 1.2f;
    //const float epicMult = 1.4f;
    //const float legendaryMult = 1.6f;
    //const float ultraLegendaryMult = 2f;

    protected abstract void ChargeWeapon();
    protected abstract void MaxChargeWeapon();
    protected abstract void ReleaseChargedWeapon();
    protected abstract void WeaponOnCD();
    public abstract void WeaponSetStats();
    protected abstract SpriteRenderer GetSpriteRenderer();

    private void Start()
    {
        player = GetComponentInParent<Player>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = wColor;
       
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = animator;
        
    }

    void Update()
    {
        if (!GameManager.instance.player.GetComponent<Player>().stunned)
        {
            UpdateTimeUntilNextAttack();
            // if (!isFantoming) setNumAttack();
            if (isFantoming)
            {
                AttackFantome(numAttack);
            }
            else Attack();
        }
    }
    public void SetRarity()
    {
        float randomR = NbRand(0, 100);
        if (!GameManager.instance.inLevel)
        {
            randomR = Mathf.RoundToInt(randomR / 1.35f);
        }
        for(int i = 0; i < possibleRarities.Length; i++)
        {
            if(randomR <= possibleRarities[i].chance)
            {
                thisRarity = possibleRarities[i];
            }
        }
    }
    public void setNumAttack()
    {
        if (Input.GetKey(chargeAttackKey)) numAttack = 1;
        else if (Input.GetKeyUp(chargeAttackKey)) numAttack = 2;
        else numAttack = 0;
        
    }
    public void Attack()//int attack)
    {
        numAttack = 0;
        if (timeUntilNextAttack <= 0)
        {

            if (Input.GetKey(chargeAttackKey) && (currentChargeTime < chargeTime))
            {
                numAttack = 1;
                currentChargeTime += Time.deltaTime;
                ChargeWeapon();
            }
            else if (Input.GetKey(chargeAttackKey) && (currentChargeTime >= chargeTime))
            {
                numAttack = 1;
                MaxChargeWeapon();
            }
            else if (Input.GetKeyUp(chargeAttackKey))
            {
                numAttack = 2;
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
    public void AttackFantome(int attack)
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
      //  Debug.Log(chargeDoneRatio);
        //Debug.Log(attackDamage * attackDamageChargedBonus * chargeDoneRatio);
       // Debug.Log(attackDamage + Mathf.FloorToInt((attackDamage * attackDamageChargedBonus * chargeDoneRatio)));
        if (currentChargeTime < chargeTime)
        {
            cible.RecevoirDegats(attackDamage + Mathf.FloorToInt((attackDamage * attackDamageChargedBonus * chargeDoneRatio)), (cible.gameObject.transform.position - transform.position).normalized, knockBackAmount,0);
        }
        else
        {
            cible.RecevoirDegats(attackDamage + Mathf.RoundToInt(attackDamageChargedBonus * attackDamage), (cible.gameObject.transform.position - transform.position).normalized, knockBackAmount,0);
        }

        if (isFire)
        {
          cible.Burn(chanceBurnProc, burnSuffered, burnDuration);
            
        }
        if (isIce)
        {
                cible.Slow(chanceSlowProc,slowValue, slowDuration, slowFadeState);
                   
        }
    }


    protected void facingMouse()
    {
        if(!isFantoming) GetComponentInParent<Player>().doFaceMouse(true);
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
        return Mathf.FloorToInt(Random.Range(min, max));
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
    public virtual bool CanSwitch()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            return true;
        }
        return false;

    }
    public IEnumerator DropWeaponAnim(float speed)
    {
       GetSpriteRenderer().sortingOrder = 1;
        while (speed > 0.0005f)
        {
            transform.position += -Vector3.up * speed;
            speed -= 0.4f * Time.deltaTime;
            yield return null;
        }
        GetSpriteRenderer().sortingOrder = 0;
    }

}

    
   

