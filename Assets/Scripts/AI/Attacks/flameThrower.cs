﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameThrower : Attacks {

    new BoxCollider2D attackHitbox;
    private Vector3 direction;
    Animator anim;

     Vector2[] f1 = {
        new Vector2(1.5f, 0),
        new Vector2(3, 3.5f)
    };
    Vector2[] f2 = {
        new Vector2(3.6f, 0),
        new Vector2(7.2f, 3.5f)
    };
    Vector2[] f3 = {
        new Vector2(3.8f, 0),
        new Vector2(7.75f, 4)
    };
    void Start()
    {
        attackHitbox = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(attack());
    }

    
    public void Setup(Vector3 dir, float damMult, float kbMult)
    {
        attackDamage = Mathf.RoundToInt(attackDamage * damMult);
        direction = dir;
        maxKnockBackAmount *= kbMult;
        this.gameObject.transform.right = direction;
    }
    //public void Setup(Vector3 dir, int dam, float kb, float range, float it)
    //{
    //    attackDamage = dam;
    //    maxKnockBackAmount = kb;
    //    attackRange = range;
    //    immuneTime = it;
    //    this.gameObject.transform.right = direction;
    //}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackHitbox.enabled = false;
            Player player = other.gameObject.GetComponent<Player>();
            Debug.Log(maxKnockBackAmount);
            player.RecevoirDegats(attackDamage, direction, maxKnockBackAmount, immuneTime);

        }
       
    }

    IEnumerator attack()
    {
        attackHitbox.offset = f1[0];
        attackHitbox.size = f1[1];
        yield return new WaitForSeconds(0.05f / anim.speed);
        attackHitbox.offset = f2[0];
        attackHitbox.size = f2[1];
        yield return new WaitForSeconds(0.05f / anim.speed);
        attackHitbox.offset = f3[0];
        attackHitbox.size = f3[1];
        yield return new WaitForSeconds(0.1f / anim.speed);
        attackHitbox.enabled = false;
        yield return new WaitForSeconds(0.30f / anim.speed);
        Destroy(this.gameObject);
    }
}