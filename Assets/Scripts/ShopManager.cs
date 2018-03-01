using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public static bool shopWantsToOpen = false;
    public GameObject canvas;
    public int prixItem = 2;
     


    private void Start()
    {
        canvas.SetActive(false);
    }
    private void OpenShop()
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
    private void AcheterItem()
    {
        if(GameManager.instance.coinCount >= prixItem)
        {
            //BuyButton(1).interactable = true;

            //if()
            //{
            //    GameManager.instance.coinCount -= prixItem;
            //}
        }
    }
    private void FixedUpdate()
    {
        //si le joueur appuy sur z et qu'il collide avec un tag de Sylvain, le shop ouvre
        OpenShop();
      
    }
}
