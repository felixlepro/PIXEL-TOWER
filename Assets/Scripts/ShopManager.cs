using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public static bool shopWantsToOpen = false;
    public GameObject canvas;
    

    private void Start()
    {
        canvas.SetActive(false);
    }
    private void openShop()
     {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(shopWantsToOpen)
            {
                canvas.SetActive(true);
                shopWantsToOpen = false;
            }
            else
            {
                canvas.SetActive(false);
                shopWantsToOpen = true;
            }
        }
    }
   
private void FixedUpdate()
    {
        //si le joueur appuy sur z et qu'il collide avec un tag de Sylvain, le shop ouvre
        openShop();
          
    }
}
