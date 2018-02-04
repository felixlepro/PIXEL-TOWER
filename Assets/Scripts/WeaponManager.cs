using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager: MonoBehaviour
{
    public GameObject player;
    public Weapon weapon;
    private SpriteRenderer spriteR;
    private Animator anim;


    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = weapon.idleSprite;
        //spriteR.color = weapon.wColor;
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = weapon.animator;
        weapon.setUpAS();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("PlayerAttack");
        }
    }

}
