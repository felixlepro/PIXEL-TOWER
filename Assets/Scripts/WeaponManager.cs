using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager: MonoBehaviour
{
    public GameObject player;
    public Weapon weapon ;
    private SpriteRenderer spriteR;
    private Animator anim;
    private BoxCollider2D coll;
    private float timeElapsed;
    private float time;

    void Start()
    {
        weapon.Attack();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = weapon.wColor;
        coll = gameObject.GetComponent<BoxCollider2D>();
        coll.enabled = false;
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = weapon.animator;
        weapon.setUpAS();
        Debug.Log(weapon.GetDamage());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colision");
        if (other.tag == "Enemy")
        {
            Debug.Log("En");
            StateController enemyScript = other.gameObject.GetComponentInParent<StateController>();
            EnvoyerDegat(enemyScript.enemy);
        }
    }

    public void EnvoyerDegat(Enemy cible)
    {
        Debug.Log(weapon.GetDamage());
        cible.recevoirDegats(weapon.GetDamage());
        
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && CheckIfCountDownElapsed(weapon.attackSpeed))
        {
            attack();
            coll.enabled = true;
        }
        else
        {
            coll.enabled = false;
        }
        
    }

    void attack()
    {
            anim.SetTrigger("PlayerAttack");
    }

     bool CheckIfCountDownElapsed(float duration)
    {
        timeElapsed += Time.deltaTime;
        return (timeElapsed >= duration);
    }

}
