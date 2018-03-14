using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    public RuntimeAnimatorController animChest;
    public int nbCoins;


    [HideInInspector] public Animator anim;
    public GameObject coinGO;

    private void Start()
    {
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
    }
}
