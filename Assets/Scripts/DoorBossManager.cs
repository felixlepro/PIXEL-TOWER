using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBossManager : MonoBehaviour {

    Animator[] anim;
    Collider2D[] coll;
    bool closed = false;

	void Start () {
        anim = GetComponentsInChildren<Animator>();
        coll = GetComponents<Collider2D>();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!closed)
        {
            if (other.tag == "Player")
            {
                closed = true;
                tag = "Obstacle";
                GameManager.instance.piggy.transform.position = GameManager.instance.player.transform.position;
                foreach (Animator an in anim)
                {
                    an.SetTrigger("Close");
                }
                foreach (Collider2D co in coll)
                {
                    co.enabled = !co.enabled;
                }
                Invoke("AAI", 1);
               
            }
        }
    }

    void AAI()
    {
        GameManager.instance.ActivateAI(true);
    }
}
