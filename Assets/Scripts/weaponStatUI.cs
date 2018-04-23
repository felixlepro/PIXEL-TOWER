using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class weaponStatUI : MonoBehaviour {

    Text statText;

    private void Start()
    {
        statText = GetComponent<Text>();
    }
     public void SetStat(WeaponManager wm)
    {
        string rar;
        if (wm.thisRarity.name == null)
        {
            rar = "Basique";
        }
        else
        {
            rar = wm.thisRarity.name;
        }
        statText.text = " Rareté: " + rar + "\n Dégât: " + wm.attackDamage + "\n Bonus de charge: ";
        if (wm.attackDamageChargedBonus != 0)
        {
            statText.text += wm.attackDamageChargedBonus.ToString("F1");
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
