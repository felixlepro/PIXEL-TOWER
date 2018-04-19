using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour {

    public float slowDownAmount;

   public float lifeTime;
    float time = 0;
     bool UpdateSpeed = true;
    private int damage;
    public  float speedBolt;
    public float maxSpeedBolt;
    private float knockBack;
    private Vector3 direction;
    private Rigidbody2D boltRigidbody;
    Collider2D collider;
    bool hasCollided = false;

    // Use this for initialization
    void Start () {
        boltRigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

	// Update is called once per frame
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

    public void Setup(int dam, Vector3 dir, float kb, float speed, float maxSpeed)
    {
        damage = dam;
        direction = dir;
        knockBack = kb;
        speedBolt = speed;
        maxSpeedBolt = maxSpeed;
        transform.right = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.tag == "Enemy")
        {
            if (!hasCollided)
            {
                hasCollided = true;
                collider.enabled = false;
                GetComponent<Animator>().SetTrigger("Hit");
                transform.right = -direction;
                EnemyManager em = other.gameObject.GetComponentInParent<EnemyManager>();
                damage = Mathf.RoundToInt(damage * speedBolt / maxSpeedBolt);
                em.RecevoirDegats(damage, direction, knockBack * speedBolt / maxSpeedBolt, 0);
                //Destroy(this.gameObject);
                UpdateSpeed = false;
                transform.parent = em.gameObject.transform;
            }

        }
        else if ((other.tag == "Obstacle" && other.isTrigger) || other.tag == "Chest")
        {
            if (!hasCollided)
            {
                hasCollided = true;
                collider.enabled = false;
            GetComponent<Animator>().SetTrigger("Hit");
            transform.right = -direction;
            //Destroy(this.gameObject);
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
