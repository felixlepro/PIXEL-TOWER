using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour {

    public static bool shopOpened = false;
    public GameObject shopInfo;
    public GameObject shopMenu;

    void Start()
    {
        shopInfo.SetActive(false);
        shopMenu.SetActive(false);
       
    }

    
    
    private void OpenShopMenu()
    {
        shopMenu.SetActive(true);
        shopOpened = true;
    }

    private void CloseShopMenu()
    {
        shopMenu.SetActive(false);
        shopOpened = false;
    }
    public void OpenShopInfo()
    {
        shopInfo.SetActive(true);
       
    }

    public void CloseShopInfo()
    {
        shopInfo.SetActive(false);

    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Z))
        {

            if (shopOpened)
            {
                CloseShopMenu();
            }
            else
            {
                OpenShopMenu();
            }

        }

    }

}