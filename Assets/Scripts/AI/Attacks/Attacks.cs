using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour {

    public int attackDamage;
    public float immuneTime;
    public float maxKnockBackAmount;
    public float attackRange;
    public float attackCD;
    [HideInInspector] public float timeUntilNextAttack;
    [HideInInspector] public Collider2D[] attackHitbox;
    public GameObject prefab;
    public float burnChance;
    public float freezeChance;

    public virtual float speed { get; set; }

    public void UpdatecurrentAttackCD()
    {
        if (timeUntilNextAttack > 0)
        {
            timeUntilNextAttack -= Time.deltaTime;
        }
    }
    public bool checkIfAttackIsReady()
    {
        return (timeUntilNextAttack <= 0);
    }
    public void resetAttackCD()
    {
        timeUntilNextAttack = attackCD;
    }

}
