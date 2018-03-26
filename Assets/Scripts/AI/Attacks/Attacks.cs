using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour {

    public int attackDamage;
    public float immuneTime;
    public float maxKnockBackAmount;
    public float attackRange;
    [HideInInspector] public Collider2D[] attackHitbox;
    public GameObject prefab;
    //abstract public void Setup(Vector3 dir, float damMult, float kbMult, float speedMult);
    //abstract public void Setup(Vector3 dir, float damMult, float kbMult);
}
