using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float speed;
    public float rotationBuffer;
    public float restartDelay = 1f;

    private Rigidbody2D playerRigidbody;
    private BoxCollider2D boxCollider;
    private Animator anim;
    Vector3 movement;

    Transform weaponTransform;
    SpriteRenderer graphicsSpriteR;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren < Animator > ();

        weaponTransform = transform.Find("WeaponRotation");
        graphicsSpriteR = GetComponentInChildren< SpriteRenderer>();
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);//C'EST LA QU'ON CHANGE LA SCENE CALISS
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartDelay);
            enabled = false;
        }

        else if ((other.tag == "Sylvain") & (Input.GetKeyDown("Enter")))
        {
           
        }

    }




        void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Move(horizontal, vertical);

        faceMouse();
    }

    void FixedUpdate()
    {
        
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

        if (direction.x < 0 && transform.localScale == faceRight && angle >= rotationBuffer & angle <= 180 - rotationBuffer)
        {
            transform.localScale = faceLeft;
            weaponTransform.localScale = new Vector3(-1, -1, 0);
            
            //weaponTransform.position = new Vector3(weaponTransform.transform.position.x - 2, weaponTransform.transform.position.y, weaponTransform.transform.position.z);

        }
        else if (direction.x > 0 & transform.localScale == faceLeft & angle >= rotationBuffer & angle <= 180 - rotationBuffer)
        {
            transform.localScale = faceRight;
            weaponTransform.localScale = new Vector3(1, 1, 0);
           
            //weaponTransform.position = new Vector3(weaponTransform.transform.position.x + 2, weaponTransform.transform.position.y, weaponTransform.transform.position.z);

        }

        if (angle > 115) graphicsSpriteR.sortingOrder = 1;
        else graphicsSpriteR.sortingOrder = -1;

        anim.SetFloat("DirectionAngle", angle);
        weaponTransform.right = direction;


        

    }

}

           
            