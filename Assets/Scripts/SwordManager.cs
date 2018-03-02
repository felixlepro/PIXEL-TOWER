using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager: MonoBehaviour
{
    //public GameObject player;
   [HideInInspector ] public Weapon  weapon;
    private SpriteRenderer spriteR;
    private Animator anim;
    private BoxCollider2D coll;

    private float chargeDoneRatio;
    private float timeUntilNextAttack;
    private float time;
    private float currentChargeTime;
    public KeyCode chargeAttackKey;

    void Start()
    {
        weapon = GetComponentInParent<Player>().player.weapon;
     //   weapon.Attack();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = weapon.wColor;
        coll = gameObject.GetComponent<BoxCollider2D>();
        coll.enabled = false;
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = weapon.animator;     
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colision");
        if (other.tag == "Enemy")
        {
            Debug.Log("En");
            StateController enemyScript = other.gameObject.GetComponentInParent<StateController>();
            EnvoyerDegat(enemyScript.enemyManager);
        }
    }

    public void EnvoyerDegat(EnemyManager cible)
    {
        if (currentChargeTime < weapon.chargeTime)
        {
            
            cible.recevoirDegats(weapon.attackDamage + Mathf.FloorToInt(weapon.attackDamageChargedBonus*chargeDoneRatio * chargeDoneRatio));
        }

        else 
        {
            cible.recevoirDegats(weapon.attackDamageChargedBonus + weapon.attackDamage);
        }
    }

    void Update()
    {
        UpdateTimeUntilNextAttack();

        if (timeUntilNextAttack <= 0)
        {

            if (Input.GetKey(chargeAttackKey) && (currentChargeTime < weapon.chargeTime))
            {
                currentChargeTime += Time.deltaTime;
                chargeDoneRatio = (currentChargeTime / weapon.chargeTime);
                anim.speed = 1+ (chargeDoneRatio * chargeDoneRatio * 1.5f);
                anim.SetBool("AttackCharge", true);

            }
            else if (Input.GetKey(chargeAttackKey) && (currentChargeTime >= weapon.chargeTime))
            {
                anim.SetBool("AttackCharge", true);
                anim.speed = 2.5f;
            }
            else if (Input.GetKeyUp(chargeAttackKey))
            {
                coll.enabled = true;
                attack();
            }
           
        }
        else
        {
            coll.enabled = false;
        }

        //if (Input.GetMouseButtonDown(0) && (timeUntilNextAttack <= 0))
        //{
        //    attack();
        //    coll.enabled = true;
        //}
        //else
        //{
        //    coll.enabled = false;
        //}

    }
    
    void attack()
    {
        anim.speed = 1;
        anim.SetBool("AttackCharge", false);
        anim.SetBool("AttackChargeMax", false);
        anim.SetTrigger("PlayerAttack");
        ResetAttackTimer();
        currentChargeTime = 0;
        GetComponentInParent<Player>().doFaceMouse(false);
        Invoke("facingMouse", anim.GetCurrentAnimatorStateInfo(0).length * anim.GetCurrentAnimatorStateInfo(0).speed);
    }
    void facingMouse()
    {
        GetComponentInParent<Player>().doFaceMouse(true);
    }

    void UpdateTimeUntilNextAttack()
    {
        if (timeUntilNextAttack > 0)
        {
            timeUntilNextAttack -= Time.deltaTime;
        }
    }
    private void ResetAttackTimer()
    {
        timeUntilNextAttack = weapon.attackSpeed;
    }


}
