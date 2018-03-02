using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class Player : MonoBehaviour {
    
    public float rotationBuffer;
    public float restartDelay = 1f;
    public int valuePerCoin = 1;
    public Text coinText;
    public int coins;
    // public float weaponDistance = 1.25f;
    public PlayerObject player;
    public GameObject weaponObject;

    [HideInInspector] public Vector2 direction;
    private Rigidbody2D playerRigidbody;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private int hp;
    private Vector3 movement;
    private bool FacingMouse = true;

    Transform weaponTransform;
    //GameObject weaponChild;
    SpriteRenderer graphicsSpriteR;

    [HideInInspector] public float timeUntilNextAttack;

    [HideInInspector] public float knockBackAmount = 0;
    [HideInInspector] public float knockBackAmountOverTime = 1;
    [HideInInspector] public float knockBackAmountOverTimeMinimum = 0.85f;
    [HideInInspector] public float knockBackTime = 1;
    [HideInInspector] public Vector3 knockBackDirection;
    [HideInInspector] public Color couleurKb = Color.white;

    void Start()
    {
        
        playerRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = player.animator;
        //player.hp = GameManager.instance.playerHp;

        weaponTransform = transform.Find("WeaponRotation");
       //weaponObject = Instantiate(weaponObject, Vector3.zero, Quaternion.identity) as GameObject;
       // weaponObject.transform.parent = weaponTransform;
       // weaponObject.name = "Weapon";

        graphicsSpriteR = GetComponentInChildren< SpriteRenderer>();
        coins = GameManager.instance.coinCount;
        coinText.text = "Coins: " + coins;
    }
    private void OnDisable()
    {
        GameManager.instance.playerHp = hp;
        GameManager.instance.coinCount = coins;
    }
    public void gainCoin()
    {
        coins += valuePerCoin;
        coinText.text = "Coins: " + coins;
    }
    void FixedUpdate()
    {
        if (knockBackAmountOverTime >= knockBackAmountOverTimeMinimum)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Move(horizontal, vertical);
            FaceMouse();
        }
        else
        {
            knockBack();
        }
    }
    public void knockBack()
    {
        float curve = (1 - knockBackAmountOverTime) * (1 - knockBackAmountOverTime);
        //Debug.Log(curve);

        graphicsSpriteR.color = new Color(1f, 1 - curve, 1 - curve, 1f);

        Vector3 kb = knockBackDirection.normalized * knockBackAmount * curve * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + kb);
        // knockBackTime /= knockBackAmount;
        knockBackAmountOverTime += Time.deltaTime * knockBackTime;

        if (knockBackAmountOverTime > knockBackAmountOverTimeMinimum)
        {
            graphicsSpriteR.color = new Color(1f, 1, 1, 1f);
        }
    }

    public void RecevoirDegats(int dammage, Vector3 kbDirection, float kbAmmount)
    {
        CameraShaker.Instance.ShakeOnce(dammage * 0.25f, 8f, 0.1f, 1f);
        player.hp -= dammage;
        if (kbAmmount != 0)
        {
            knockBackDirection = kbDirection;
            knockBackAmount = kbAmmount;
            knockBackAmountOverTime = 0;
        }
        graphicsSpriteR.color = new Color(1f, 0, 0, 1f);
    }


  
    private void Restart()
    {
        if (GameManager.instance.inLevel)
        {
            SceneManager.LoadScene("SceneShop", LoadSceneMode.Single);
            GameManager.instance.inLevel = false;
        }
        else
        {
            SceneManager.LoadScene("SceneLevel", LoadSceneMode.Single);
            GameManager.instance.inLevel = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartDelay);
            enabled = false;
        }
        if ((other.tag == "sylvain"))
        {
            ShopManager.shopWantsToOpen = true;
        }
        if (other.tag == "Coin")
        {
            gainCoin();
            other.gameObject.SetActive(false);
        }

    }
    


    
    private void Move(float h, float v)
    {

        movement.Set(h, v, 0f);
        movement = movement.normalized * player.speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
        if (h == 0 && v == 0)
        {
            anim.SetBool("IsMoving", false);
        }
        else anim.SetBool("IsMoving", true);
    }


    void FaceMouse()
    {
        Debug.Log("caca");
        if (FacingMouse)
        {
            Debug.Log("caca2");
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
    }
    public void doFaceMouse(bool fm)
    {
        FacingMouse = fm;
    }
}
