using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInfoScript : MonoBehaviour
{
    public GameObject panel;
    public void Start()
    {
        panel.SetActive(false); 
    }

    public void OpenInf()
    {
       panel.SetActive(true);
    }

    public void CloseInf()
    {
       panel.SetActive(false);
    }
}