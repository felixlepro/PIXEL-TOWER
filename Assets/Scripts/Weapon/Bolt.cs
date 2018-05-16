using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour {

    public float slowDownAmount;

   public float lifeTime;
    float time = 0;
     bool UpdateSpeed = true;
    public  float speedBolt;
    public float maxSpeedBolt;
    private Vector3 direction;
    private Rigidbody2D boltRigidbody;
    Collider2D colliderD;
    bool hasCollided = false;


    WeaponManager weaponSource;

    void Start () {
        boltRigidbody = GetComponent<Rigidbody2D>();
        colliderD = GetComponent<Collider2D>();
    }

	void FixedUpdate () {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
            StartCoroutine(fadeToDeath());
        }
        if (UpdateSpeed)
        {
            speedBolt -= slowDownAmount * Time.deltaTime;
            if (speedBolt > 2)
            {
                direction = direction.normalized * speedBolt * Time.deltaTime;
                boltRigidbody.MovePosition(transform.position + direction);
            }
            else Destroy(this.gameObject);
        }
    }

        public void Setup(Vector3 dir, float speed, float maxSpeed, WeaponManager wm)
    {
        direction = dir;
        speedBolt = speed;
        maxSpeedBolt = maxSpeed;
        transform.right = direction;
        weaponSource = wm;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            
            if (!hasCollided)
            {
                hasCollided = true;
                colliderD.enabled = false;
                GetComponent<Animator>().SetTrigger("Hit");
                transform.right = -direction;
                EnemyManager em = other.gameObject.GetComponentInParent<EnemyManager>();
                weaponSource.EnvoyerDegat(other.GetComponentInParent<EnemyManager>());
            UpdateSpeed = false;
                transform.parent = em.gameObject.transform;
            }

        }
        else if ((other.tag == "Obstacle" && other.isTrigger) || other.tag == "Chest")
        {
            if (!hasCollided)
            {
                hasCollided = true;
                colliderD.enabled = false;
            GetComponent<Animator>().SetTrigger("Hit");
            transform.right = -direction;
            UpdateSpeed = false;
            }
        }
    }

    IEnumerator fadeToDeath()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        while (sprite.color.a > 0)
        {
            Color tmp = sprite.color;
            tmp.a -= Time.deltaTime/4;
            sprite.color = tmp;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
