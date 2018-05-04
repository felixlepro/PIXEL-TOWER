using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour {

    const float speedDrop = 4;
    private static GameObject[] weaponList;
    private static GameObject coinP;
    private static GameObject key;
    private static PiggyManager piggy;
    private static GameObject time;

    public static void Initialize()    {
        weaponList = GameManager.instance.weapons;
        coinP = GameManager.instance.coinPrefab;
        piggy = GameManager.instance.piggy.GetComponent<PiggyManager>();
        //GameObject.Find("Piggy").GetComponent<PiggyManager>(); //
        key = GameManager.instance.key;
        time = GameManager.instance.timeObject;
        //GameObject.Find("Piggy").GetComponent<PiggyManager>();
    }

	public static void DropWeapon(GameObject weapon,Vector3 pos, bool anim) //if the weapon is already generated
    {
        
        WeaponManager wp = weapon.GetComponent<WeaponManager>();
        wp.enabled = false;
        //weapon.GetComponent<Collider2D>().enabled = true;
        weapon.SetActive(false);                    //Prévient une erreur étrange
        weapon.transform.parent = null;
        weapon.SetActive(true);
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.position = pos;
        weapon.transform.localScale = new Vector3(weapon.transform.localScale.x, Mathf.Abs(weapon.transform.localScale.y), 1);
        if (anim)
        {
            wp.StartCoroutine(wp.DropWeaponAnim(speedDrop));
        }
    }
    public static void DropRandomWeapon(Vector3 pos, bool anim) //if you want the weapon to be generated
    {
        
        GameObject weapon = Instantiate(weaponList[Random.Range(0, weaponList.Length)], pos, Quaternion.identity);
       WeaponManager wp = weapon.GetComponent<WeaponManager>();
        wp.WeaponSetStats();
        weapon.transform.localScale = wp.baseScale*0.4f;
        wp.enabled = false;
        if (anim)
        {
            wp.StartCoroutine(wp.DropWeaponAnim(speedDrop));
        }
    }
    public static void DropCoin(Vector3 pos,int nbrCoin)
    {    
        for (int i = 0; i < nbrCoin; i++)
        {
            piggy.coinList.Add(Instantiate(coinP, pos, Quaternion.identity));
        }
    }
    public static void DropKey(Vector3 pos)
    {
        Instantiate(key, pos, Quaternion.identity);
    }
    public static void DropTime(Vector3 pos)
    {
        Instantiate(time, pos, Quaternion.identity);
    }
}
