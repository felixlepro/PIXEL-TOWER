﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerclePuissant : Attacks {

    public const float chargeTime = 0.5f;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        attackHitbox = GetComponents<Collider2D>();
        StartCoroutine(attack());
        
    }


    //public void Setup(Vector3 dir, float damMult, float kbMult)
    //{
    //    attackDamage = Mathf.RoundToInt(attackDamage * damMult);
    //    direction = dir;
    //    maxKnockBackAmount *= kbMult;
    //    this.gameObject.transform.right = direction;
    //}
    public void Setup(Vector3 dir, int dam, float kb, float range, float it, float burn, int burnDa,float burnDu, float slow, float slowAm, float slowDu, float freeze, float freezeDu)
    {
        attackDamage = dam;
        maxKnockBackAmount = kb;
        attackRange = range;
        immuneTime = it;
        burnChance = burn;
        burnDamage = burnDa;
        burnDuration = burnDu;
        slowChance = slow;
        slowAmount = slowAm;
        slowDuration = slowDu;

        freezeChance = freeze;
        freezeDuration = freezeDu;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackHitbox[0].enabled = false;
            Player player = other.gameObject.GetComponent<Player>();
            player.RecevoirDegats(attackDamage, Vector3.zero, maxKnockBackAmount, immuneTime);
            player.Burn(burnChance,burnDamage, burnDuration);
            player.Slow(slowChance, slowAmount, slowDuration,false);
            player.Freeze(freezeChance, freezeDuration);
        }

    }

    IEnumerator attack()
    {
        yield return new WaitForSeconds(chargeTime);
        attackHitbox[0].enabled = true;
        yield return null;
        attackHitbox[0].enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
