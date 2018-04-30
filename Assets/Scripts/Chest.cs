using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    public RuntimeAnimatorController animChest;
    public int nbCoins;
    public int weaponDropChance;
    public int timeDropChange;
   // Collider2D col;
    [HideInInspector] public bool hasKey = false;
    [HideInInspector] public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = animChest;
    }
    public void OpenChest()
    {
        anim.SetTrigger("Open");
        DropManager.DropCoin(transform.position,nbCoins);
        if (weaponDropChance > Random.value *100)  {
           DropManager.DropRandomWeapon(transform.position - Vector3.up / 3, true);
        }
        if (hasKey)
        {
            DropManager.DropKey(transform.position);
        }
        if (timeDropChange > Random.value * 100)
        {
            DropManager.DropTime(transform.position);
        }
      //  Destroy(col);
    }
}
