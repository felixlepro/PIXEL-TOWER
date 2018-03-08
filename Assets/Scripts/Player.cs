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
    public PlayerObject player;
    private bool immune = false;

    [HideInInspector] public Vector2 direction;
    private Rigidbody2D playerRigidbody;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private int hp;
    private Vector3 movement;
    private bool FacingMouse = true;

    Transform weaponTransform;
    SpriteRenderer graphicsSpriteR;
    SpriteRenderer weaponSprite;

    [HideInInspector] public float timeUntilNextAttack;

    [HideInInspector] public float knockBackAmount = 0;
    [HideInInspector] public float knockBackAmountOverTime = 1;
    [HideInInspector] public float knockBackAmountOverTimeMinimum = 0.85f;
    [HideInInspector] public float knockBackTime = 1;
    [HideInInspector] public Vector2 knockBackDirection;
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

        graphicsSpriteR = GetComponentInChildren<SpriteRenderer>();
        weaponSprite = weaponTransform.gameObject.GetComponentInChildren<SpriteRenderer>();
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
    }

    

    public void RecevoirDegats(int damage, Vector3 kbDirection, float kbAmmount, float immuneTime)
    {
        if (!immune)
        {
            CameraShaker.Instance.ShakeOnce(damage * 0.25f, 2.5f, 0.1f, 1f);
            hp -= damage;
            if (kbAmmount != 0)
            {
                knockBackDirection = kbDirection;
                knockBackAmount = kbAmmount;
                knockBackAmountOverTime = 0;
                StartCoroutine("KnockBack");
            }
            immune = true;
            StartCoroutine("ImmuneAnim");
            Invoke("StopImmunity", immuneTime);
        }
        
    }
    IEnumerator KnockBack()
    {
        graphicsSpriteR.color = new Color(1f, 0, 0, graphicsSpriteR.color[3]);
        while (knockBackAmountOverTime < knockBackAmountOverTimeMinimum)
        {
            float curve = (1 - knockBackAmountOverTime) * (1 - knockBackAmountOverTime);
            graphicsSpriteR.color = new Color(1f, 1 - curve, 1 - curve, graphicsSpriteR.color[3]);

         //   Debug.Log(knockBackDirection.normalized);

            Vector3 kb = knockBackDirection.normalized * knockBackAmount * curve * Time.deltaTime;
            playerRigidbody.MovePosition(transform.position + kb);
            knockBackAmountOverTime += Time.deltaTime * knockBackTime;
            yield return null;
        }
        graphicsSpriteR.color = new Color(1f, 1, 1, graphicsSpriteR.color[3]);

    }

    IEnumerator ImmuneAnim()
    {
        float alpha = 0.225f;
       // float curve;
        float time = 0;
        while (immune)
        {
            time += Time.deltaTime;
            if (alpha == 1f)
            {
                alpha = 0.2f;
            }
            else alpha = 1;
            //curve = Mathf.Cos(time*2000*Mathf.PI)/3 + 0.75f;
            //Debug.Log(curve);
            graphicsSpriteR.color = new Color(graphicsSpriteR.color[0], graphicsSpriteR.color[1], graphicsSpriteR.color[2], alpha);
            weaponSprite.color = new Color(graphicsSpriteR.color[0], graphicsSpriteR.color[1], graphicsSpriteR.color[2], alpha);
            yield return new WaitForSeconds(0.075f);
        }
    }

    private void StopImmunity()
    {
        immune = false;
        graphicsSpriteR.color = new Color(graphicsSpriteR.color[0], graphicsSpriteR.color[1], graphicsSpriteR.color[2], 1f);
        weaponSprite.color = new Color(graphicsSpriteR.color[0], graphicsSpriteR.color[1], graphicsSpriteR.color[2], 1f);
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
        if (FacingMouse)
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
    }
    public void doFaceMouse(bool fm)
    {
        FacingMouse = fm;
    }
}
