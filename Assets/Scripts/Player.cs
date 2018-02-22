using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float speed;
    public float rotationBuffer;
    public float restartDelay = 1f;
    public float weaponDistance = 1.25f;

    [Range(0f, 1f)]
    public float ratioWeaponPivot;
    public Vector2 direction;
    


    public Weapon weapon;

    private Rigidbody2D playerRigidbody;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private int hp;

    private float knockBackAmount = 0;
    private float knockBackAmountOverTime = 1;
    private float knockBackAmountOverTimeMinimum = 0.85f;
    private const float knockBackMultiplier = 20;
    private float knockBackTime = 1;
    private Vector3 knockBackDirection;

    private Vector3 movement;

    Transform weaponTransform;
    GameObject weaponChild;
    SpriteRenderer graphicsSpriteR;

 

    void Start()
    {
        
        playerRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren < Animator > ();
        hp = GameManager.instance.playerHp;
        weaponTransform = transform.Find("WeaponRotation");
        weaponChild = GameObject.Find("Weapon");
        graphicsSpriteR = GetComponentInChildren< SpriteRenderer>();

        
    }
    void FixedUpdate()
    {
        if (knockBackAmountOverTime >= knockBackAmountOverTimeMinimum)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Move(horizontal, vertical);
        }
        else
        {
            knockBack();
        }

        faceMouse();
    }

    public void RecevoirDegats(int dammage, Vector3 kbDirection, float kbAmmount)
    {
        hp -= dammage;
        knockBackDirection = kbDirection;
        knockBackAmount = kbAmmount;
        knockBackAmountOverTime = 0;
      //  Debug.Log("Player:  " + hp);
    }


    
    private void OnDisable()
    {
        GameManager.instance.playerHp = hp;
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartDelay);
            enabled = false;
        }
        if (other.tag == "enemy")   
        {

        }
        
    }
    private void knockBack()
    {
        Vector3 kb = knockBackDirection.normalized * knockBackAmount * Time.deltaTime * knockBackMultiplier * (1 - knockBackAmountOverTime) * (1 - knockBackAmountOverTime);
        playerRigidbody.MovePosition(transform.position + kb);
       // knockBackTime /= knockBackAmount;
        knockBackAmountOverTime += Time.deltaTime * knockBackTime;
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
        direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
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
    void setWeaponPivot()
    {
        weaponTransform.position = new Vector3(ratioWeaponPivot * weaponDistance, weaponTransform.position.y, weaponTransform.position.z);
        weaponChild.transform.position = new Vector3(weaponDistance - (ratioWeaponPivot * weaponDistance) + weaponTransform.position.x, weaponChild.transform.position.y, weaponChild.transform.position.z);
    }

    //private Vector3 vecteurUnitaire(Vector3 vecteur)
    //{
    //    if (vecteur.x == 0 && vecteur.y ==0 &&vecteur.z == 0)
    //    {
    //        vecteur /= vecteur.magnitude;
    //    }
    //    return vecteur;
    //}


}

           
            