using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour {
    public const float lifeTime = 30;
    float time = 0;
    private int damage;
    private float speedBall;
    float maxSpeedBolt;
    private float knockBack;
    private Vector3 direction;
    private Rigidbody2D ballRigidbody;
    Collider2D collider;

    // Use this for initialization
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
            Destroy(this.gameObject);
        }
       direction = direction.normalized * speedBall * Time.deltaTime;
       ballRigidbody.MovePosition(transform.position + direction);

        
    }

    public void Setup(int dam, Vector3 dir, float kb, float speed, float maxSpeed)
    {
        damage = dam;
        direction = dir;
        knockBack = kb;
        speedBall = speed;
        maxSpeedBolt = maxSpeed;
        //this.gameObject.transform.right = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            collider.enabled = false;
            EnemyManager em = other.gameObject.GetComponentInParent<EnemyManager>();
            em.recevoirDegats(damage, direction, knockBack * speedBall / maxSpeedBolt);

            Animator anim = GetComponent<Animator>();
            anim.SetTrigger("Hit");
            Invoke("destroyObject", anim.GetCurrentAnimatorClipInfo(0).Length);

        }
        else if (other.tag == "Obstacle" && !other.isTrigger)
        {
            collider.enabled = false;
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
