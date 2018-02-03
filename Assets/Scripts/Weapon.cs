using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject player;
    private Animator anim;
    public GameObject weapon;
    public float rotationAmount;
    private Rigidbody2D weaponRigidbody;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        weaponRigidbody = weapon.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        faceMouseNew();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    playerAttack();
        //}
        //faceMouse();
    }

    void faceMouse()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - player.transform.position.x, mousePosition.y - player.transform.position.y);

        transform.up = direction;
    }

    void playerAttack()
    {
        anim.SetTrigger("PlayerAttack");
    }

    void faceMouseNew() {
        float directionOfRotation;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 directionMouse = new Vector2(mousePosition.x - player.transform.position.x, mousePosition.y - player.transform.position.y);
        //Vector2 directionWeapon = new Vector2(Mathf.Cos((weapon.transform.rotation.z+90)*Mathf.PI / 180), Mathf.Sin((weapon.transform.rotation.z+90) * Mathf.PI / 180));

        float angleWeapon = weapon.transform.eulerAngles.z;

        float angleMouse = Vector2.Angle(directionMouse, new Vector2(0, 1));
        if (directionMouse.x > 0) angleMouse = 360 - angleMouse;

        float angle = angleMouse - angleWeapon;
      
        weaponRigidbody.transform.position = transform.position;
        if (angle < 0) angle = (angle * angle*-1);
        else angle = angle * angle;

        weaponRigidbody.angularVelocity = (angle * rotationAmount * Time.deltaTime );

        Debug.Log(angleMouse + "    " + angleWeapon  + "    " +   angle);
    }

}
