using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileManager : MonoBehaviour {

    SpriteRenderer[] spriteR;
    public Sprite sprite1;
    public Sprite sprite2;

    void Start()
    {

        spriteR = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer caca in spriteR)
        {
            int rng = Random.Range(0, 2);
            if (0 == rng)
            {
                caca.sprite = sprite1;
            }
            else
            {
                caca.sprite = sprite2;
            }
        } 
	}
}
