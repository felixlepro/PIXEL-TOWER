using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {
    public WeaponManager weapon;
    public GameObject panelInfo;
    public Text infoText;
    
	void Start () {
        infoText.text = "Attack : " + weapon.attackDamage + "\nRange : " + "grosCaca";
        panelInfo.SetActive(false);
    }

    public void PanelInfoPop()
    {
        panelInfo.SetActive(true);
    }

    public void PanelInfoDepop()
    {
        panelInfo.SetActive(false);
    }

}
