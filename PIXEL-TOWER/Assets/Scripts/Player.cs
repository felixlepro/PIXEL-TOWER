using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed;
    private Rigidbody2D playerRigidbody;
    private Animator anim;
    Vector3 movement;

	void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	void Update () {
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
        if (h  == 0 && v == 0)
        {
            anim.SetBool("IsMoving", false);
        }
        else anim.SetBool("IsMoving", true);
    }


        void faceMouse()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        float angle = Vector2.Angle(direction, new Vector2(0, -1));

        System.Console.WriteLine(angle);
        System.Console.WriteLine("caca");
        print(angle);
        anim.SetFloat("DirectionAngle", angle);


        if (angle > 30 && angle <= 160)
        {
            if (direction.x >= 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else transform.localScale = new Vector3(-1, 1, 1);
        }

    }
}
