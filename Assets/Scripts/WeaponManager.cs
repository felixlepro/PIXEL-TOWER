using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager: MonoBehaviour
{
    public GameObject player;
    public Weapon weapon;
    private SpriteRenderer spriteR;
    private Animator anim;
    private float timeElapsed;


    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = weapon.wColor;
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = weapon.animator;
        weapon.setUpAS();
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
