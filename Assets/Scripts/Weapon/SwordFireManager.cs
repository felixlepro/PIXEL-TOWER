using UnityEngine;
using UnityEditor;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;



public class SwordFireManager : SwordManager
{
    public int chanceBurnProc;
    public int burnTime;

    public EnemyManager enemy;
    private Vector3 vZero = new Vector3(0, 0, 0);
    private float flZero = 0f;
    int burnChances;
    int nbRand;
    int burnTimer;

    public SwordFireManager()
    {
        wColor = Color.yellow;
        attackDamage = 20;
        chanceBurnProc = 30;
        burnTime = 4;
    }
    private void Start()
{
    burnChances = chanceBurnProc;
    burnTimer = burnTime;
    nbRand = Rand();
    Debug.Log(nbRand);
}


private void MightBurn()
{
    if (Rand() <= burnChances)
    {
        StartCoroutine("BurnThemAlive", 0.5);

    }
}

IEnumerator BurnThemAlive()
{
    float time = 0;

    while (time < burnTimer)
    {
        time += Time.deltaTime;
        BurnPerSec();
        yield return new WaitForSeconds(1f);
    }
}

public void BurnPerSec()
{
    enemy.recevoirDegats(5, vZero, 0f);
}

public int Rand()
{
    int dabNbRand = Random.Range(0, 100);
    return dabNbRand;
}
 
}