using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager: MonoBehaviour
{
    public GameObject player;
    public Weapon weapon;
    private SpriteRenderer spriteR;

    public Rigidbody2D weaponRB;

    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = weapon.idleSprite;
        //spriteR.color = weapon.wColor;
        weaponRB = GetComponent<Rigidbody2D>();
           
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            weapon.attack(weaponRB);
        }    
    }

 

  

   

}
