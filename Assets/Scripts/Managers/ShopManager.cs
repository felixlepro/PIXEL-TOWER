using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject canvas;
    public bool openIt = false;
    private bool canEnter = true;
    private string tagP;

    private void Start()
    {
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


}