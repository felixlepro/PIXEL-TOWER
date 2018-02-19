using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    public static bool ShopWantsToOpen;
    public GameObject panel;

    void OpenShop()
    {
        
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (ShopWantsToOpen)
            {
                panel.SetActive(true);
            }
        }
        
    }
}
