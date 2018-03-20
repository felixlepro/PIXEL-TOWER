using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;


[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class PlayerObject : ScriptableObject  {
    public int hp;
    public float maxSpeed;
    public GameObject weaponObject;
    public WeaponManager weapon;
    public RuntimeAnimatorController animator;

 
}
