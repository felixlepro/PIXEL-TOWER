using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour {

    SpriteRenderer[] spriteR;
    public Sprite[] spritee;


    void Start()
    {

        spriteR = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer caca in spriteR)
        {
            int rng = Random.Range(0, spritee.Length);
            caca.sprite = spritee[rng];
            int rng2 = Random.Range(0, 3);
            caca.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90 * rng2);
        } 
	}
}
