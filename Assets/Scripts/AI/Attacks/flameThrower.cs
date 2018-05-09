using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flameThrower : MonoBehaviour {

    BoxCollider2D attackHitbox;
    private Vector3 direction;
    Animator anim;
    Attacks attackStats;

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
    public void Setup(Attacks at, Vector3 dir)
    {
        attackStats = at;
        direction = dir;

        attackHitbox = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        this.gameObject.transform.right = direction;
        StartCoroutine(attack());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackHitbox.enabled = false;
            Player player = other.gameObject.GetComponent<Player>();
            player.SeFaitAttaquer(attackStats, direction);
           
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




//public void Setup(Vector3 dir, int dam, float kb, float range, float it, float burn, int burnDa, float burnDu, float slow, float slowAm, float slowDu)
//{
//    direction = dir;
//    attackDamage = dam;
//    maxKnockBackAmount = kb;
//    attackRange = range;
//    immuneTime = it;
//    burnChance = burn;
//    Debug.Log(burn);
//    burnDamage = burnDa;
//    burnDuration = burnDu;
//    slowChance = slow;
//    slowAmount = slowAm;
//    slowDuration = slowDu;

//    this.gameObject.transform.right = direction;


//}
//player.RecevoirDegats(attackDamage, direction, maxKnockBackAmount, immuneTime);
//player.Burn(burnChance, burnDamage, burnDuration);
//player.Slow(slowChance, slowAmount, slowDuration, false);
