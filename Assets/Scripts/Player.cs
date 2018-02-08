﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed;
    public float rotationBuffer;
    private Rigidbody2D playerRigidbody;
    private Animator anim;
    Vector3 movement;

    //Transform playerGraphics;
    Transform weaponTransform;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren < Animator > ();

        //playerGraphics = transform.Find("Graphics");
        weaponTransform = transform.Find("WeaponRotation");

    }

    void Update()
    {
       
        faceMouse();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Move(horizontal, vertical);
    }
    private void Move(float h, float v)
    {

        movement.Set(h, v, 0f);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
        if (h == 0 && v == 0)
        {
            anim.SetBool("IsMoving", false);
        }
        else anim.SetBool("IsMoving", true);
    }


    void faceMouse()
    {
        Vector3 faceRight = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
        Vector3 faceLeft = new Vector3(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        float angle = Vector2.Angle(direction, new Vector2(0, -1));

        anim.SetFloat("DirectionAngle", angle);
        weaponTransform.up = direction;


        if (direction.x < 0 && transform.localScale == faceRight && angle >= rotationBuffer & angle <= 180 - rotationBuffer)
        {
            transform.localScale = faceLeft;
            //playerGraphics.localScale = faceLeft;
            //weaponTransform.localScale = faceLeft;
            //weaponTransform.position = new Vector3(weaponTransform.transform.position.x - 2, weaponTransform.transform.position.y, weaponTransform.transform.position.z);

        }
        else if (direction.x > 0 & transform.localScale == faceLeft & angle >= rotationBuffer & angle <= 180 - rotationBuffer)
        {
            transform.localScale = faceRight;
            //playerGraphics.localScale = faceRight;
            //weaponTransform.localScale = faceRight;
            //weaponTransform.position = new Vector3(weaponTransform.transform.position.x + 2, weaponTransform.transform.position.y, weaponTransform.transform.position.z);

        }

    }

}

           
            