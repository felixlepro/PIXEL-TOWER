using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;



public class SwordFireManager : SwordManager
{
    public SwordFire sf;
    public Enemy enemy;
    int burnChances;
    int nbRand;
    int enemyhp;
    private void Start()
    {
        burnChances = sf.chanceProc;
        nbRand = Rand();
        Debug.Log(nbRand); 
    }
   

    private void MightBurn()
    {
        if(Rand() <= burnChances)
        {


        }
    }

    //IEnumerator BurnthemAlive()
    //{
    //    float time = 0;
       
    //    while (time < sf.chanceProc)
    //    {
    //        time += Time.deltaTime;
            
    //        yield return new WaitForSeconds(1f);

    //    }
    //}

    public int Rand()
    {    
        int dabNbRand = Random.Range(0,100);
        return dabNbRand;
    }

}