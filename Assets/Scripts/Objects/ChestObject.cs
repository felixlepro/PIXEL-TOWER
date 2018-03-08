using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Chest", menuName = "Chest")]
abstract public class ChestObject : ScriptableObject
{
    public RuntimeAnimatorController  animChest;
    public int nbCoins;

}
