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

    public bool isFire = false;
    public bool isIce = false;

    [HideInInspector]public int chanceBurnProc = 30;
    [HideInInspector] int chanceSlowProc = 40;

    [HideInInspector] public float burnDuration = 4;
    [HideInInspector] public int burnSuffered = 5;

    [HideInInspector] public int slowDuration = 3;
    [HideInInspector] public float slowValue = 0.3f;

   [HideInInspector] bool slowFadeState = false;

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
            if (NbRand() < chanceBurnProc)
            {
                cible.Burn(burnDuration,burnSuffered);
            }
        }
        else if (isIce)
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

    
   

//Attributs responsables des effets de Burn et de Slow (propre à chaque arme)


    //protected int NbRand()
    //{
    //    rand = Random.value*100;
    //    return Mathf.FloorToInt(rand);
    //}


  //  public float attackSpeed; //  attackCD
   

//Attributs responsables des effets de Burn et de Slow (propre à chaque arme)
    //public int chanceBurnProc = 30;
    //public int chanceSlowProc = 40;

    //public int burnDuration = 4;
    //public int burnSuffered = 5;

    //public int slowDuration = 3;
    //public float slowValue = 0.3f;

    //public bool slowFadeState = false;

    //public bool IsFire = false;
    //public bool IsIce = false;
 //Fin des attributs d'effets spéciaux d'armes  -Simon

 //       }
 ////Vérification de la présence ou non d'un effet spéciale sur l'arme et appel de la fonction appropriée dans EnemyManager si le joueur à un nombre aléatoire qui respecte la condition d'activation.
 //       if (isFire)
 //       {
 //           if (NbRand() < chanceBurnProc)
 //           {
 //               cible.Burn(burnDuration,burnSuffered);
 //       }
 //       if (isIce)
 //       {
 //           if (NbRand() < chanceSlowProc)
 //           {
 //                cible.Slow(slowValue,slowDuration,slowFadeState);
 //           }          
 //       }
 //   }


 //   }
//Génère un nombre random
//    protected int NbRand()
//    {
//        rand = Random.value*100;
//        return Mathf.FloorToInt(rand);
//    }
//}

  //  public float attackSpeed; //  attackCD

   



//Attributs responsables des effets de Burn et de Slow ainsi que la rareté (propre à chaque arme)

    //public int chanceBurnProc;
    //public int chanceSlowProc;



    //public int burnDuration;

    //public int burnSuffered;



    //public int slowDuration;

    //public float slowValue;



    //public bool slowFadeState;



    //public bool isFire;

    //public bool isIce;

    //rareté

    

 //Fin des attributs d'effets spéciaux d'armes  -Simon


