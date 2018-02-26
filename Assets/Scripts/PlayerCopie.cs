using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCopie : MonoBehaviour
{
    public float speed;
    public float rotationBuffer;
    public float restartDelay = 1f;
    public float weaponDistance = 1.25f;
    [Range(0f, 1f)]
    public float ratioWeaponPivot;
    public float angle;
    public List<PositionPlus> chemin = new List<PositionPlus>();

    public Weapon weapon;

    private Rigidbody2D playerRigidbody;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private int hp;
    Vector3 movement;

    Transform weaponTransform;
    GameObject weaponChild;
    SpriteRenderer graphicsSpriteR;

    // Costructeur
    public void Initialize(List<PositionPlus> listPos)
    {
        chemin = listPos;
    }


    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        hp = GameManager.instance.playerHp;
        weaponTransform = transform.Find("WeaponRotation");
        weaponChild = GameObject.Find("Weapon");
        graphicsSpriteR = GetComponentInChildren<SpriteRenderer>();


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

    }
    

    void FixedUpdate()
    {
        if (chemin.Count > 0)
        {
            float horizontal = chemin[0].position.x;
            float vertical = chemin[0].position.y;
            Move(horizontal, vertical);
            faceMouse(chemin[0].direction);
            chemin.RemoveAt(0);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
    private void Move(float h, float v)
    {
       Vector3 nouvPlace =new Vector3(h, v);

        if (transform.position == nouvPlace )
        {
            anim.SetBool("IsMoving", false);
        }
        else anim.SetBool("IsMoving", true);
        transform.position = nouvPlace;
    }


    void faceMouse(Vector3 direction)
    {
        Vector3 faceRight = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
        Vector3 faceLeft = new Vector3(-Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
        
        angle = Vector2.Angle(direction, new Vector2(0, -1));

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


}
