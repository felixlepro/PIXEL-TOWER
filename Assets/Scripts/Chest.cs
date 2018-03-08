using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    [HideInInspector] public Animator animChest;
     public ChestObject chest;
    public GameObject coinGO;

    private void Start()
    {
        animChest = GetComponent<Animator>();
        animChest.runtimeAnimatorController = chest.animChest;
    }
    public void OpenChest()
    {
        animChest.SetTrigger("Open");
        for (int coin =0; coin < chest.nbCoins; coin ++)
        {
            Instantiate(coinGO, transform.position, Quaternion.identity);
        }
    }
}
