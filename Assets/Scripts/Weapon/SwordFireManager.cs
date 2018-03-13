using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;



public class SwordFireManager : SwordManager
{
    public SwordFire sf;
    public Enemy enemy;
    public EnemyManager enemyM;
    private Vector3 vZero = new Vector3(0,0,0);
    private float flZero = 0f;
    int burnChances;
    int nbRand;
    int enemyhp;
    int burnTimer;
    private void Start()
    {
        burnChances = sf.chanceProc;
        burnTimer = sf.burnTime;
        nbRand = Rand();
        Debug.Log(nbRand); 
    }
   

    private void MightBurn()
    {
        if(Rand() <= burnChances)
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
        enemyM.recevoirDegats(5,vZero,0f);
    }

    public int Rand()
    {    
        int dabNbRand = Random.Range(0,100);
        return dabNbRand;
    }

}