using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : Attacks {
    public const float lifeTime = 30;
    float time = 0; 
    private float knockBack;
    public Vector3 direction;
    public Vector3 direction2;
    private Rigidbody2D ballRigidbody;
    Collider2D attackHitbox;
    public float speedBall;



    // Use this for initialization
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        attackHitbox = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
            Destroy(this.gameObject);
        }
        Debug.Log(direction);
        ballRigidbody.MovePosition(transform.position + direction.normalized * speedBall * Time.deltaTime);

    }

    public void Setup(Vector3 dir, float damMult, float kbMult, float speedMult)
    {
        attackDamage = Mathf.RoundToInt(attackDamage*damMult);
        direction = dir;
        direction2 = dir;

        knockBack *= kbMult;
        speedBall *= speedMult;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackHitbox.enabled = false;
            Player player = other.gameObject.GetComponent<Player>();
            player.RecevoirDegats(attackDamage, direction, knockBack, immuneTime);

            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("Hit");
            Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);
        }
        else if (other.tag == "EnnemieManager")
            {
                attackHitbox.enabled = false;
                EnemyManager em = other.gameObject.GetComponent<EnemyManager>();
                em.recevoirDegats(attackDamage, direction, knockBack);

                Animator anim = GetComponent<Animator>();
                anim.SetTrigger("Hit");
                Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);
            }

        else if ((other.tag == "Obstacle" && !other.isTrigger) || other.tag == "Chest")
        {
            attackHitbox.enabled = false;
            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("Hit");
            Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);

        }
    }

    void destroyObject()
    {
        Destroy(this.gameObject);
    }

}
