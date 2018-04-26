using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> weaponList;
    public List<Image> imList;
    public List<Button> BuList;
    public List<Text> pInf;
   


    public GameObject[] weaponTypes;
    public GameObject canvas;
    private GameObject panel;

     Player p1;

    private int armBpos;
    public bool openIt = false;
    private bool canEnter = true;
    private string tagP;
    private int currentWtype;
    public int iMax = 6;

    private void Start()
    {
        p1 = GameManager.instance.player.GetComponent<Player>();
        p1.gainKey();
        //weaponList.Clear();
        while (weaponList.Count < iMax)
        {
            CreateWeapon();       
        }

        SetInfo();
      
    }


    private void FixedUpdate()
    {
        if (tagP =="Player")
        {
            if (Input.GetKey(KeyCode.Z)&&(canEnter))
            {
                canEnter = false;

                if(openIt)
                {
                    canvas.SetActive(false);
                    openIt = false;
                }
                else
                {
                    canvas.SetActive(true);
                    openIt = true;
                }
            }
            if (!Input.GetKey(KeyCode.Z))
            {

               canEnter = true;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            tagP = "Player";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            tagP = "";
        }
    }
    private int RandomTypeWeapon()
    {
        currentWtype = Random.Range(0,weaponTypes.Length-1);
        return currentWtype;       
    }
    private void CreateWeapon()
    {     
        weaponList.Add(Instantiate(weaponTypes[RandomTypeWeapon()],Vector3.zero, Quaternion.identity));
        weaponList[weaponList.Count - 1].GetComponent<WeaponManager>().WeaponSetStats();
        weaponList[weaponList.Count - 1].SetActive(false);
    }

    private void SetInfo()
    {
        for (int n = 0; n < iMax; n++)
        {
           imList[n].sprite = weaponList[n].GetComponent<WeaponManager>().sprite;
           string rar = "Basique";

            if (weaponList[n].GetComponent<WeaponManager>().thisRarity.name != null)
            {
                rar = weaponList[n].GetComponent<WeaponManager>().thisRarity.name;
            }
            pInf[n].text = " Rareté: " + rar + "\n Dégât: " + weaponList[n].GetComponent<WeaponManager>().attackDamage + "\n Bonus de charge: ";
            if (weaponList[n].GetComponent<WeaponManager>().attackDamageChargedBonus != 0)
            {
                pInf[n].text += (1 + weaponList[n].GetComponent<WeaponManager>().attackDamageChargedBonus).ToString("F1");
            }
            pInf[n].text += "\n Vitesse: " + (1 / weaponList[n].GetComponent<WeaponManager>().attackSpeed).ToString("F1") + "\n Recul: " + weaponList[n].GetComponent<WeaponManager>().knockBackAmount.ToString("F1");
            if (weaponList[n].GetComponent<WeaponManager>().isFire)
            {
                pInf[n].text += "\n Feu";
            }
            if (weaponList[n].GetComponent<WeaponManager>().isIce)
            {
                pInf[n].text += "\n Glace";
            }
            pInf[n].text += "\n Coût: " + weaponList[n].GetComponent<WeaponManager>().cost;

        }
    }
    public void BuyWeapon()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "BuyButton1": armBpos = 0;break;
            case "BuyButton2": armBpos = 1; break;
            case "BuyButton3": armBpos = 2; break;
            case "BuyButton4": armBpos = 3; break;
            case "BuyButton5": armBpos = 4; break;
            case "BuyButton6": armBpos = 5; break;

        }
        
        if (p1.GetComponent<Player>().coins >= weaponList[armBpos].GetComponent<WeaponManager>().cost)
        {
            p1.GetComponent<Player>().coins -= weaponList[armBpos].GetComponent<WeaponManager>().cost;
            p1.ChangeWeapon(weaponList[armBpos]);

        }
        else
        {
            Debug.Log("gros pauvre");
        }
    }
}