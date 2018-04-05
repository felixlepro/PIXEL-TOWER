using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<GameObject> weaponList;

    public GameObject[] weaponTypes;
    public GameObject canvas;

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
}