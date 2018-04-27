using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class weaponStatUI : MonoBehaviour {

    Text statText;

     public void SetStat(WeaponManager wm)
    {
        statText = GetComponent<Text>();
        string rar = "Basique";
 
        if (wm.thisRarity.name != null)
        {
            rar = wm.thisRarity.name;
        }
        statText.text = " Rareté: " + rar + "\n Dégât: " + wm.attackDamage;
        if (wm.attackDamageChargedBonus != 0)
        {
            statText.text +="\n Bonus de charge: " + (1+wm.attackDamageChargedBonus).ToString("F1");
        }
        statText.text += "\n Vitesse: " + (1 / wm.attackSpeed).ToString("F1") + "\n Recul: " + wm.knockBackAmount.ToString("F1");
        if (wm.isFire)
        {
            statText.text += "\n Feu";
        }
        if (wm.isIce)
        {
            statText.text += "\n Glace";
        }
    }
}
