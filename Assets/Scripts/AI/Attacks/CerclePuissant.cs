using System.Collections;
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
    public void Setup(Vector3 dir, int dam, float kb, float range, float it, float burn, float freeze)
    {
        attackDamage = dam;
        maxKnockBackAmount = kb;
        attackRange = range;
        immuneTime = it;
        burnChance = burn;
        freezeChance = freeze;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackHitbox[0].enabled = false;
            Player player = other.gameObject.GetComponent<Player>();
            player.RecevoirDegats(attackDamage, Vector3.zero, maxKnockBackAmount, immuneTime);

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
