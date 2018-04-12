﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class Player : Character {

    public GameObject[] startingWeapon;
    const int weaponEquipedMax = 2;
    public List<WeaponManager> weaponList = new List<WeaponManager>();
    public float rotationBuffer;
    [HideInInspector] public float restartDelay = 1f;
    [HideInInspector] public int valuePerCoin = 1;
    public Text coinText;
    public int coins;
    public Image hpBar;
    public GameObject gameOverMenu; //je pense pas que ca devrait etre dans player ca

    [HideInInspector] public int currentWeaponIndex = 0;
    [HideInInspector] public Vector2 direction;
    private Rigidbody2D playerRigidbody;
    private BoxCollider2D boxCollider;
    private Animator anim;
    [HideInInspector] public Vector3 movement;
    private bool FacingMouse = true;
    bool rooted;
    Transform weaponTransform;
    SpriteRenderer graphicsSpriteR;
    SpriteRenderer weaponSprite;

    [HideInInspector] public float timeUntilNextAttack;

    const float timePerKnockBackAmount = 20; //10 kba lasts 1 seconds
    [HideInInspector] public float knockBackAmount = 0;
    [HideInInspector] public float knockBackAmountOverTime = 1;
    [HideInInspector] public float knockBackAmountOverTimeMinimum = 0.7f;
    [HideInInspector] public float knockBackTime = 1;
    [HideInInspector] public Vector2 knockBackDirection;
    [HideInInspector] public Color couleurKb = Color.white;

    void Start()
    {
        hp = maxHp;
        hpBar.fillAmount = (float)hp / (float)maxHp;
        currentSpeed = maxMoveSpeed;
        playerRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        //player.hp = GameManager.instance.playerHp;

        weaponTransform = transform.Find("WeaponRotation");
        //weaponObject = Instantiate(weaponObject, Vector3.zero, Quaternion.identity) as GameObject;
        // weaponObject.transform.parent = weaponTransform;
        // weaponObject.name = "Weapon";
        //ChangeWeapon(player.weaponObject);

        graphicsSpriteR = transform.Find("Graphics").GetComponent<SpriteRenderer>();
        coins = GameManager.instance.coinCount;
        coinText.text = "Coins: " + coins;

        foreach (GameObject sw in startingWeapon)
        {
            GameObject newWeapon = Instantiate(sw, Vector3.zero, Quaternion.identity) as GameObject;
            ChangeWeapon(newWeapon);
        }
    }
    private void Update()
    {
        if (!stunned) {
            if(Input.GetKeyDown("left shift"))
            {
                SwitchWeapon();
            }
         }
    }
    void FixedUpdate()
    {     
        if (!stunned)
        {
            FaceMouse();
            if (!rooted)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                Move(horizontal, vertical);
            }
            
        }
    }

    private void Move(float h, float v)
    {
     
        movement.Set(h, v, 0f);
        movement = movement.normalized * currentSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
        if (h == 0 && v == 0)
        {
            anim.SetBool("IsMoving", false);
        }
        else anim.SetBool("IsMoving", true);
    }


    public override void RecevoirDegats(int damage, Vector3 kbDirection, float kbAmmount, float immuneTime)
    {
        if (!immune)
        {
            DamageTextManager.CreateFloatingText(damage, transform.position);
            CameraShaker.Instance.ShakeOnce(damage * 0.25f, 2.5f, 0.1f, 1f);
            hp -= damage;        
            if(hp <= 0)
            {
                Time.timeScale = 0;
                gameOverMenu.SetActive(true);                                      
            }
                hpBar.fillAmount = (float)hp / (float)maxHp;
                if (kbAmmount != 0)
            {
                knockBackDirection = kbDirection;
                knockBackAmount = kbAmmount;
                knockBackAmountOverTime = 0;
                StartCoroutine("KnockBack");
            }
            else StartCoroutine("RedOnly");
            immune = true;
            StartCoroutine("ImmuneAnim");
            Invoke("StopImmunity", immuneTime);
        }  
    }
    public IEnumerator RedOnly()
    {
        float kbAmountOverTime = 0;
        graphicsSpriteR.color = new Color(1f, 0, 0, graphicsSpriteR.color[3]);
        while (kbAmountOverTime < knockBackAmountOverTimeMinimum)
        {
            // if (!hitAWall)
            {
                float curve = (1 - kbAmountOverTime) * (1 - kbAmountOverTime);
                graphicsSpriteR.color = new Color(1f, 1 - curve, 1 - curve, graphicsSpriteR.color[3]);

                kbAmountOverTime += Time.deltaTime * 1.75f;
            }
            yield return null;
        }
        graphicsSpriteR.color = new Color(1f, 1, 1, graphicsSpriteR.color[3]);
    }
    IEnumerator KnockBack()
    {
        yield return new WaitForFixedUpdate();
        graphicsSpriteR.color = new Color(1f, 0, 0, graphicsSpriteR.color[3]);
         rooted = true;

        //float knockBackTime = timePerKnockBackAmount / ((knockBackAmount - timePerKnockBackAmount) / 2 + timePerKnockBackAmount);
        float knockBackTime = 2 * timePerKnockBackAmount / (knockBackAmount + timePerKnockBackAmount);
        while (knockBackAmountOverTime < knockBackAmountOverTimeMinimum)
        {
            float curve = (1 - knockBackAmountOverTime) * (1 - knockBackAmountOverTime);
            graphicsSpriteR.color = new Color(1f, 1 - curve, 1 - curve, graphicsSpriteR.color[3]);

         //   Debug.Log(knockBackDirection.normalized);

            Vector3 kb = knockBackDirection.normalized * knockBackAmount * curve * Time.deltaTime;
            playerRigidbody.MovePosition(transform.position + kb);
            knockBackAmountOverTime += Time.deltaTime * knockBackTime;
            yield return new WaitForFixedUpdate();
        }
        graphicsSpriteR.color = new Color(1f, 1, 1, graphicsSpriteR.color[3]);
        rooted = false;
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

            if(weaponSprite == null)
            {
                weaponSprite = weaponTransform.gameObject.GetComponentInChildren<SpriteRenderer>();
            } 
            graphicsSpriteR.color = new Color(graphicsSpriteR.color[0], graphicsSpriteR.color[1], graphicsSpriteR.color[2], alpha);
            weaponSprite.color = new Color(weaponSprite.color[0], weaponSprite.color[1], weaponSprite.color[2], alpha);
            yield return new WaitForSeconds(0.075f);
        }
    }

    private void StopImmunity()
    {
        immune = false;
        graphicsSpriteR.color = new Color(graphicsSpriteR.color[0], graphicsSpriteR.color[1], graphicsSpriteR.color[2], 1f);
        weaponSprite.color = new Color(weaponSprite.color[0], weaponSprite.color[1], weaponSprite.color[2], 1f);
    }
  
    private void Restart()
    {
        if (GameManager.instance.inLevel)
        {
            SceneManager.LoadScene(2);
            GameManager.instance.inLevel = false;
        }
        else
        {
            SceneManager.LoadScene(1);
            GameManager.instance.inLevel = true;
        }
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

    //private void OnTriggerStay2D(Collider2D other)            //VOIR LE SCRIPT PLAYERTRIGGERS
    //{
    //    if (other.tag == "Exit")
    //    {
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            other.GetComponent<Animator>().SetTrigger("Open");
    //            Invoke("Restart", restartDelay);
    //            enabled = false;
    //        }
            
    //    }
    //    if ((other.tag == "Coin"))
    //    {
    //        gainCoin();
    //        Destroy(other.gameObject);
    //        GameObject.Find("GameManager").GetComponent<GameManager>().PlaySound(GameObject.Find("GameManager").GetComponent<GameManager>().coinSound);

    //    }
        

    //}
    
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

    public void ChangeWeapon(GameObject newWeapon)
    {
        //foreach (Transform child in weaponTransform)
        //{
        //    if (child.GetComponent<WeaponManager>().isMelee == newWeapon.GetComponent<WeaponManager>().isMelee)
        //    {
        //        GameObject.Destroy(child.gameObject);
        //    }
        //    child.gameObject.SetActive(false);
        //}
        bool foundAWeapon = false;
        newWeapon.GetComponent<WeaponManager>().enabled = true;
        newWeapon.GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < weaponList.Count; i++)
        {
                weaponList[i].gameObject.SetActive(false);
                if (weaponList[i].isMelee == newWeapon.GetComponent<WeaponManager>().isMelee)
                {
                // GameObject.Destroy(weaponList[i].gameObject);
                weaponList[i].gameObject.SetActive(true);
                WeaponDrop.DropWeapon(weaponList[i].gameObject, transform.position - Vector3.right / 3);
                    weaponList[i] = newWeapon.GetComponent<WeaponManager>();
                    currentWeaponIndex = i;
                    foundAWeapon = true;
                }
        }
        if (!foundAWeapon)
        {
            weaponList.Add(newWeapon.GetComponent<WeaponManager>());
            currentWeaponIndex = weaponList.Count-1;
        }
        //Instantiate(newWeapon);
        newWeapon.transform.parent = weaponTransform;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.transform.localScale = newWeapon.GetComponent<WeaponManager>().baseScale;
        newWeapon.transform.localPosition = newWeapon.GetComponent<WeaponManager>().basePosition;
    }
    public void SwitchWeapon()
    {
        if (weaponList[currentWeaponIndex].CanSwitch())
        {
            weaponList[currentWeaponIndex].gameObject.SetActive(false);
            currentWeaponIndex = (currentWeaponIndex + 1) % weaponList.Count; //Fait que ca loop dans la liste au lieu de dépassé
            weaponList[currentWeaponIndex].gameObject.SetActive(true);
        }
    }
    void InstantiateWeapon(GameObject prefab)
    {
        foreach (Transform child in weaponTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject weaponInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

        weaponInstance.transform.parent = weaponTransform;
        weaponInstance.transform.localScale = prefab.transform.localScale;
        weaponInstance.transform.localPosition = prefab.transform.position;

    }
    
}
