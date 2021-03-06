﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class Player : Character {
    
    public GameObject[] startingWeapon;
    const int weaponEquipedMax = 2;
    public List<WeaponManager> weaponList;
    public float rotationBuffer;
    [HideInInspector] public float restartDelay = 1f;
    [HideInInspector] public int valuePerCoin = 1;
    [HideInInspector] public float rewindCharge;
    public int maxRewindCharge;
    Image rewindBar;
    Text coinText;
    public int coins;
     Image hpBar;
    public int currentArmor;

    [HideInInspector] public bool hasKey;
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

    [HideInInspector] public weaponStatUI weaponStatUI;

    [HideInInspector] public float timeUntilNextAttack;

    const float timePerKnockBackAmount = 20; //10 kba lasts 1 seconds
    [HideInInspector] public float knockBackAmount = 0;
    [HideInInspector] public float knockBackAmountOverTime = 1;
    [HideInInspector] public float knockBackAmountOverTimeMinimum = 0.7f;
    [HideInInspector] public float knockBackTime = 1;
    [HideInInspector] public Vector2 knockBackDirection;
    [HideInInspector] public Color couleurKb = Color.white;

    //GameObject gameOverMenu;
    GameObject iconKey;
    GameObject iconArmor;
    //private void Awake()
    //{
    //    setPlayerStats();
    //}
    void Start()
    {
        hasKey = false;
        PlayerSetUp();
        canvas = GameObject.Find("Canvas");
    }
    private void Update()
    {
        if (!stunned) {
            if(Input.GetKeyDown("left shift"))
            {
               GameManager.instance.Restart();
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
    void PlayerSetUp()
    {
       // gameOverMenu = GameObject.Find("GameOverMenu");
        currentSpeed = maxMoveSpeed;
        playerRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        weaponTransform = transform.Find("WeaponRotation");

        graphicsSpriteR = transform.Find("Graphics").GetComponent<SpriteRenderer>();

            hp = maxHp;         
            hpBar.fillAmount = (float)hp / (float)maxHp;
            rewindCharge = maxRewindCharge;
            rewindBar.fillAmount = (float)rewindCharge / (float)maxRewindCharge;
            weaponList = new List<WeaponManager>();
            foreach (GameObject sw in startingWeapon)
            {
                GameObject newWeapon = Instantiate(sw, Vector3.zero, Quaternion.identity) as GameObject;
                ChangeWeapon(newWeapon);
            }          
        
    }
    public void SetUpTimeChargeUI(Image rewindB)
    {
        rewindBar = rewindB;
        if (rewindCharge < 0)
        {
            rewindCharge = 0;
        }
        rewindBar.fillAmount = (float)rewindCharge / (float)maxRewindCharge;
    }
    public void SetUpCoin(Text c)
    {
        coinText = c;
        coinText.text = "P I È C E S : " + coins;
    }
    public void SetUpHpBar(Image c)
    {
        hpBar = c.GetComponentInChildren<Image>();
        hpBar.fillAmount = (float)hp / (float)maxHp;
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
            hp -= (damage - (currentArmor));
            hpBar.fillAmount = (float)hp / (float)maxHp;
            //Debug.Log(hpBar.fillAmount);
            if (hp <= 0)
            {
                Time.timeScale = 0;
                GameManager.instance.gameOverMenu.SetActive(true);                                   
            }
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
    public void Heal(int ammount)
    {
        hp += ammount;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        hpBar.fillAmount = (float)hp / (float)maxHp;
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

    public void StopImmunity()
    {
        immune = false;
        graphicsSpriteR.color = new Color(graphicsSpriteR.color[0], graphicsSpriteR.color[1], graphicsSpriteR.color[2], 1f);
        weaponSprite.color = new Color(weaponSprite.color[0], weaponSprite.color[1], weaponSprite.color[2], 1f);
    }
  
    
    private void OnDisable()
    {
     //   GameManager.instance.playerHp = hp;
      //  GameManager.instance.coinCount = coins;
    }
    public void gainCoin()
    {
        coins += valuePerCoin;
        coinText.text = "P I È C E S : " + coins;
    }
    public void LooseCoin(int coinnbr)
    {
        coins -= coinnbr;
        coinText.text = "P I È C E S : " + coins;

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
    //        GameManager.instance..PlaySound(GameManager.instance..coinSound);

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
            direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y - 0.5f);
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
        bool foundAWeapon = false;
        newWeapon.GetComponent<WeaponManager>().enabled = true;
        newWeapon.GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < weaponList.Count; i++)
        {
                
                if (weaponList[i].isMelee == newWeapon.GetComponent<WeaponManager>().isMelee)
                {
                // GameObject.Destroy(weaponList[i].gameObject);
                weaponList[i].gameObject.SetActive(true);
                DropManager.DropWeapon(weaponList[i].gameObject, transform.position + Vector3.up/2 - Vector3.right/4,true);
                    weaponList[i] = newWeapon.GetComponent<WeaponManager>();
                    currentWeaponIndex = i;
                    foundAWeapon = true;
                }
                else
                weaponList[i].gameObject.SetActive(false);
        }
        if (!foundAWeapon)
        {
            weaponList.Add(newWeapon.GetComponent<WeaponManager>());
            currentWeaponIndex = weaponList.Count-1;
        }
        newWeapon.transform.parent = weaponTransform;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.transform.localScale = newWeapon.GetComponent<WeaponManager>().baseScale;
        newWeapon.transform.localPosition = newWeapon.GetComponent<WeaponManager>().basePosition;
        setUIWeaponpStat();
    }
    public void SwitchWeapon()
    {
        if (weaponList[currentWeaponIndex].CanSwitch())
        {
            weaponList[currentWeaponIndex].gameObject.SetActive(false);
            currentWeaponIndex = (currentWeaponIndex + 1) % weaponList.Count; //Fait que ca loop dans la liste au lieu de dépassé
            weaponList[currentWeaponIndex].gameObject.SetActive(true);
            setUIWeaponpStat();
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
    public bool GainTimeCharge(float timeC)  {
        if (rewindCharge >= maxRewindCharge)
        {
            return false;
        }

        rewindCharge += timeC;
        if (rewindCharge > maxRewindCharge)
        {
            rewindCharge = maxRewindCharge;
        }
        rewindBar.fillAmount = (float)rewindCharge / (float)maxRewindCharge;
        return true;
    }
    public bool LooseTimeCharge(float chargePerSeconds)
    {
        bool canRewind = true;      
        if (rewindCharge < 0)
        {
            rewindCharge = 0;
            canRewind = false;
        }
        rewindCharge -= chargePerSeconds * Time.deltaTime;
        rewindBar.fillAmount = rewindCharge / (float)maxRewindCharge;
        return canRewind;
    }
    public void gainKey()
    {
        hasKey = true;
        iconKey.SetActive(true);
    }

    public void SetupIconKey(GameObject icon)
    {
        iconKey = icon;
        iconKey.SetActive(false);
    }

    public void SetupIconArmor(GameObject icon)
    {
        iconArmor = icon;
        iconArmor.SetActive(true);
        iconArmor.GetComponentInChildren<Text>().text = currentArmor.ToString();
    }
    public void setUIWeaponpStat()
    {
        weaponStatUI.SetStat(weaponList[currentWeaponIndex]);
        
    }
    public void SetUpWeaponStatUI(weaponStatUI wsUI)
    {
        weaponStatUI = wsUI;
        if (GameManager.instance.level != 0)
        {
            setUIWeaponpStat();
        }
    }
    //public void setPlayerStats()
    //{
    //    if (GameManager.instance.level != 0)
    //    {

    //        hp = GameManager.playerStat.hp;
    //        Debug.Log(hp);
    //        coins = GameManager.playerStat.coins;
    //        //currentWeaponIndex = GameManager.playerStat.;
    //        startingWeapon = GameManager.playerStat.weapons;
    //    }
    //    else
    //    {
    //        hp = maxHp;
    //        coins = 0;
    //        currentWeaponIndex = 0;
    //    }
    //    //   Debug.Log("ca");
    //   // hpBar.fillAmount = (float)hp / (float)maxHp;
    //}
    //public void setPlayerStats(int h, int coin, int cwi, GameObject[] wp, bool lvl0)
    //{
    //    if (!lvl0)
    //    {
    //        Debug.Log(h);
    //        hp = h;
    //        coins = coin;
    //        currentWeaponIndex = cwi;
    //        startingWeapon = wp;
    //    }
    //    else
    //    {
    //        hp = maxHp;
    //        coins = 0;
    //        currentWeaponIndex = 0;
    //    }
    // //   Debug.Log("ca");
    //    hpBar.fillAmount = (float)hp / (float)maxHp;
    //}
    //public GameObject[] weaponObjects()
    //{
    //    GameObject[] wo = new GameObject[weaponList.Count];
    //    for (int i = 0; i< weaponList.Count; i++)
    //    {
    //        wo[i] = weaponList[i].gameObject;
    //    }
    //    return wo;

    //}

    public override Vector3 PositionIcone()
    {
        return transform.position;
    }

}
