using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : Projectile {
    public const float lifeTime = 30;
    float time = 0; 
    private float knockBack;
    public Vector3 direction;
    public Vector3 direction2;
    private Rigidbody2D ballRigidbody;
    //Collider2D attackHitbox;

    // Use this for initialization
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        attackHitbox = GetComponents<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
            Destroy(this.gameObject);
        }
        //Debug.Log(direction);
        ballRigidbody.MovePosition(transform.position + direction.normalized * speed * Time.deltaTime);

    }

    //public void Setup(Vector3 dir, float damMult, float kbMult, float speedMult)
    //{
    //    attackDamage = Mathf.RoundToInt(attackDamage * damMult);
    //    direction = dir;
    //    direction2 = dir;

    //    knockBack *= kbMult;
    //    speed *= speedMult;
    //}
    public void Setup(Vector3 dir, int dam, float kb, float range,float it, float sped, float burn, float freeze)
    {
        attackDamage = dam;
        maxKnockBackAmount = kb;
        attackRange = range;
        immuneTime = it;
        direction = dir;
        speed = sped;
        burnChance = burn;
        freezeChance = freeze;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackHitbox[0].enabled = false;
            Player player = other.gameObject.GetComponent<Player>();
            player.RecevoirDegats(attackDamage, player.transform.position - transform.position, knockBack, immuneTime);
            speed = 0;
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("Hit");
            Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);
        }
        else if (other.tag == "EnnemieManager")
            {
                attackHitbox[0].enabled = false;
                EnemyManager em = other.gameObject.GetComponent<EnemyManager>();
                em.RecevoirDegats(attackDamage, em.transform.position - transform.position, knockBack,0);
            speed = 0;
            Animator anim = GetComponent<Animator>();
                anim.SetTrigger("Hit");
                Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);
            }

        else if ((other.tag == "Obstacle" && !other.isTrigger) || other.tag == "Chest")
        {
            attackHitbox[0].enabled = false;
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("Hit");
            speed = 0;
            Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);

        }
    }

    void destroyObject()
    {
        Destroy(this.gameObject);
    }

}
