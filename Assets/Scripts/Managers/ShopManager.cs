using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> weaponList;
    public List<GameObject> panelList;
    public List<Image> imList;

    public GameObject[] weaponTypes;
    public GameObject canvas;
    private GameObject panel;
    private GameObject buttonBuy;

    public bool openIt = false;
    private bool canEnter = true;
    private string tagP;
    private int currentWtype;
    public int iMax = 6;

    private void Start()
    {
        weaponList.Clear();
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
    }

    private void SetInfo()
    {
        for (int n = 0; n < iMax; n++)
        {
            imList[n].sprite = weaponList[n].GetComponent<WeaponManager>().sprite;
        }
    }
    private void BuyWeapon()
    {

    }
}