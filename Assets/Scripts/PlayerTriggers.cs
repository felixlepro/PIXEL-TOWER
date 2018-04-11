using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour {

    Player player;

	void Start () {
        player = GetComponentInParent<Player>();
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                other.GetComponent<Animator>().SetTrigger("Open");
                player.Invoke("Restart", player.restartDelay);
                enabled = false;
            }

        }
        if ((other.tag == "Coin"))
        {
            player.gainCoin();
            Destroy(other.gameObject);
            GameObject.Find("GameManager").GetComponent<GameManager>().PlaySound(GameObject.Find("GameManager").GetComponent<GameManager>().coinSound);

        }


    }
}
