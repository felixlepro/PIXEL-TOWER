using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonImageScript : MonoBehaviour {

    public GameObject panel;
       
    
      


    private void Update()
    {
        
    }


    public void OpenInfo()
    {
        panel.SetActive(true);
    }
    public void CloseInfo()
    {
        panel.SetActive(false);
    }
}
