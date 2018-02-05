using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy enemy;
    private SpriteRenderer spriteR;
    private Animator anim;


    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = enemy.wColor;
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = enemy.animator;;
    }

    void Update()
    {


            //anim.SetTrigger("PlayerAttack");
        
    }

}
