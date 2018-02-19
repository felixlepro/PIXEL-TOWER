using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager: MonoBehaviour
{
    public GameObject player;
    public Weapon weapon ;
    private SpriteRenderer spriteR;
    private Animator anim;
    //private BoxCollider2D coll;
    private float timeElapsed;


    void Start()
    {
        weapon.Attack();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = weapon.wColor;
        //coll = gameObject.GetComponent<BoxCollider2D>();
        //coll.enabled = false;
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = weapon.animator;
        weapon.setUpAS();
        Debug.Log(weapon.GetDamage());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            StateController enemyScript = other.gameObject.GetComponent<StateController>();
            EnvoyerDegat(enemyScript.enemy);
            if (enemyScript.enemy.hp <= 0)
            {
                enemyScript.gameObject.SetActive(false);
            }
        }
    }

    public void EnvoyerDegat(Enemy cible)
    {
        cible.recevoirDegats(weapon.GetDamage());
        Debug.Log(weapon.GetDamage());
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            attack();
        }
    }

    void attack()
    {
        if (CheckIfCountDownElapsed(weapon.attackSpeed))
        {
            anim.SetTrigger("PlayerAttack");
        }
    }

     bool CheckIfCountDownElapsed(float duration)
    {
        timeElapsed += Time.deltaTime;
        return (timeElapsed >= duration);
    }

}
