using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour {

    private int damage;
    private float speedBolt;
    private float knockBack;
    private Vector3 direction;
    private Rigidbody2D boltRigidbody;

    // Use this for initialization
    void Start () {
        boltRigidbody = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update () {
        direction = direction.normalized * speedBolt * Time.deltaTime;
        boltRigidbody.MovePosition(transform.position + direction);
    }

    public void Setup(int dam, Vector3 dir, float kb, float speed)
    {
        damage = dam;
        direction = dir;
        knockBack = kb;
        speedBolt = speed;
        this.gameObject.transform.right = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            StateController enemyScript = other.gameObject.GetComponentInParent<StateController>();
            enemyScript.enemyManager.recevoirDegats(damage, direction, knockBack);
            Destroy(this.gameObject);
        }
        else if(other.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        }
    }
}
