using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    public RuntimeAnimatorController animChest;
    public int nbCoins;
    public int weaponDropChance;
    Collider2D col;

    [HideInInspector] public Animator anim;
    public GameObject coinGO;

    private void Start()
    {
        nbCoins = Random.Range(Mathf.RoundToInt(nbCoins / 2), Mathf.RoundToInt(nbCoins * 1.5f));
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = animChest;
    }
    public void OpenChest()
    {
        anim.SetTrigger("Open");
        for (int coin =0; coin < nbCoins; coin ++)
        {
            Instantiate(coinGO, transform.position, Quaternion.identity);
        }
        if (weaponDropChance < Random.value *100)  {
            WeaponDrop.DropRandomWeapon(transform.position- Vector3.up / 3);
        }
        Destroy(col);
    }
}
