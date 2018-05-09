using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour {
    public const float lifeTime = 30;
    float time = 0; 
    public Vector3 direction;
    //public Vector3 direction2;
    private Rigidbody2D ballRigidbody;
    Attacks attackStats;
    Collider2D attackHitbox;

    void Update()
    {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
            Destroy(this.gameObject);
        }

    }

    public void Setup(Attacks at , Vector3 dir)
    {
        attackStats = at;
        direction = dir;

        ballRigidbody = GetComponent<Rigidbody2D>();
        attackHitbox = GetComponent<Collider2D>();
        ballRigidbody.velocity = direction.normalized * at.speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackHitbox.enabled = false;
            Player player = other.gameObject.GetComponent<Player>();
            player.SeFaitAttaquer(attackStats , direction);
          
            ballRigidbody.velocity = Vector3.zero;
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("Hit");
            Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);
        }
        //else if (other.tag == "EnnemieManager")
        //    {
        //        attackHitbox[0].enabled = false;
        //        EnemyManager em = other.gameObject.GetComponent<EnemyManager>();
        //        em.RecevoirDegats(attackDamage, em.transform.position - transform.position, knockBack,0);
        //    speed = 0;
        //    Animator anim = GetComponent<Animator>();
        //        anim.SetTrigger("Hit");
        //        Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);
        //    }

        else if ((other.tag == "Obstacle" && other.isTrigger) || other.tag == "Chest")
        {
            attackHitbox.enabled = false;
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("Hit");
            ballRigidbody.velocity = Vector3.zero;
            Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);

        }
    }

    void destroyObject()
    {
        Destroy(this.gameObject);
    }

}

//player.RecevoirDegats(attackDamage, player.transform.position - transform.position, maxKnockBackAmount, immuneTime);
//player.Burn(burnChance, burnDamage, burnDuration);
//player.Slow(slowChance, slowAmount, slowDuration, false);
//public void Setup2(Vector3 dir, int dam, float kb, float range,float it, float sped, float burn, int burnDa, float burnDu, float slow, float slowAm, float slowDu)
//{
//    attackDamage = dam;
//    maxKnockBackAmount = kb;
//    attackRange = range;
//    immuneTime = it;
//    direction = dir;
//    speed = sped;

//    burnChance = burn;
//    burnDamage = burnDa;
//    burnDuration = burnDu;
//    slowChance = slow;
//    slowAmount = slowAm;
//    slowDuration = slowDu;

//    ballRigidbody = GetComponent<Rigidbody2D>();
//    attackHitbox = GetComponents<Collider2D>();
//    ballRigidbody.velocity = direction.normalized * speed;
//}
