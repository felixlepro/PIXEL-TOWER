using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour {

    Player player;
    PiggyManager pm;
    bool onExit = false;
    bool onWeapon = false;

    void Start () {
        player = GetComponentInParent<Player>();
        pm = GameManager.instance.piggy.GetComponent<PiggyManager>();
       // GameObject.Find("Piggy").GetComponent<PiggyManager>();
        //
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Weapon")
        {
            if (Input.GetKey(KeyCode.E) && player.weaponList[player.currentWeaponIndex].CanSwitch())
            {
                player.ChangeWeapon(other.gameObject);
            }

        }
        if (other.tag == "Exit")
        {
            if (player.hasKey && Input.GetKey(KeyCode.E))
            {
                player.hasKey = false;
                other.GetComponent<Animator>().SetTrigger("Open");
                GameManager.instance.Invoke("Restart", player.restartDelay);
                enabled = false;
            }

        }

        if ((other.tag == "Coin")) {

                pm.coinList.Remove(other.gameObject);
                pm.coinList.TrimExcess();
                player.gainCoin();
                Destroy(other.gameObject);
                // GameManager.instance..PlaySound(GameManager.instance..coinSound);
                GameManager.instance.PlaySound(GameManager.instance.coinSound);
            
        }
        if ((other.tag == "Key"))
        {
            player.gainKey();
            Destroy(other.gameObject);
            GameManager.instance.PlaySound(GameManager.instance.coinSound);
        }
        if ((other.tag == "TimeCharge"))
        {
           if (player.GainTimeCharge(35)) { 
            Destroy(other.gameObject);
                //GameManager.instance.PlaySound(GameManager.instance.coinSound);
            }
        }


    }
}
