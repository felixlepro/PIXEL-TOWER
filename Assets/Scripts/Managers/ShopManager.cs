using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> weaponList;
    public List<GameObject> panelList;

    public GameObject[] weaponTypes;
    private GameObject canvas;
    private GameObject panel;
    private Image weaponIm;
    private GameObject buttonBuy;

    public bool openIt = false;
    private bool canEnter = true;
    private string tagP;
    private int currentWtype;
    public int weaponMax = 6;

    private void Start()
    {
        while(weaponList.Count < weaponMax)
        {
            CreateWeapon();
        }  
        SetInfo(weaponList[0]);
        canvas.SetActive(false);
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
        currentWtype = Random.Range(0,weaponTypes.Length);
        return currentWtype;         
    }
    private void CreateWeapon()
    {
        RandomTypeWeapon();
        weaponList.Add(Instantiate(weaponTypes[currentWtype],Vector3.zero, Quaternion.identity));
        weaponList[weaponList.Count - 1].GetComponent<WeaponManager>().WeaponSetStats();
    }

    private void SetInfo(GameObject weap)
    {
        weaponIm.sprite = weap.GetComponent<WeaponManager>().sprite;

    }
}