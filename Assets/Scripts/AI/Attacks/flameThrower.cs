using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameThrower : Attacks {

    BoxCollider2D attackHitbox2;
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
        attackHitbox2 = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(attack());
    }

    
    //public void Setup(Vector3 dir, float damMult, float kbMult)
    //{
    //    attackDamage = Mathf.RoundToInt(attackDamage * damMult);
    //    direction = dir;
    //    maxKnockBackAmount *= kbMult;
    //    this.gameObject.transform.right = direction;
    //}
    public void Setup(Vector3 dir, int dam, float kb, float range, float it, float burn, int burnDa, float burnDu, float slow, float slowAm, float slowDu, float freeze, float freezeDu)
    {
        direction = dir;
        attackDamage = dam;
        maxKnockBackAmount = kb;
        attackRange = range;
        immuneTime = it;
         burnChance = burn;
        //Debug.Log(burn);
        burnDamage = burnDa;
        burnDuration = burnDu;
        slowChance = slow;
        slowAmount = slowAm;
        slowDuration = slowDu;

        freezeChance = freeze;
        freezeDuration = freezeDu;
         this.gameObject.transform.right = direction;
    
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackHitbox2.enabled = false;
            Player player = other.gameObject.GetComponent<Player>();
            player.RecevoirDegats(attackDamage, direction, maxKnockBackAmount, immuneTime);
            player.Burn(burnChance, burnDamage, burnDuration);
            player.Slow(slowChance, slowAmount, slowDuration, false);
            player.Freeze(freezeChance, freezeDuration);
        }
       
    }

    IEnumerator attack()
    {
        attackHitbox2.offset = f1[0];
        attackHitbox2.size = f1[1];
        yield return new WaitForSeconds(0.05f / anim.speed);
        attackHitbox2.offset = f2[0];
        attackHitbox2.size = f2[1];
        yield return new WaitForSeconds(0.05f / anim.speed);
        attackHitbox2.offset = f3[0];
        attackHitbox2.size = f3[1];
        yield return new WaitForSeconds(0.1f / anim.speed);
        attackHitbox2.enabled = false;
        yield return new WaitForSeconds(0.30f / anim.speed);
        Destroy(this.gameObject);
    }
}
