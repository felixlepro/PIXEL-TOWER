using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour {

	public static void DropWeapon(GameObject weapon,Vector3 pos) //if the weapon is already generated
    {
        WeaponManager wp = weapon.GetComponent<WeaponManager>();
        wp.enabled = false;
        weapon.GetComponent<Collider2D>().enabled = true;
        weapon.transform.parent = null;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.position = pos;
    }
    public static void DropRandomWeapon(Vector3 pos) //if you want the weapon to be generated
    {
        GameObject[] weaponList = GameObject.Find("GameManager").GetComponent<GameManager>().weapons;
        GameObject weapon = Instantiate(weaponList[Random.Range(0, weaponList.Length)], pos, Quaternion.identity);
        WeaponManager wp = weapon.GetComponent<WeaponManager>();
        wp.WeaponSetStats();
        weapon.transform.localScale = wp.baseScale*0.4f;
        wp.enabled = false;
    }

}
