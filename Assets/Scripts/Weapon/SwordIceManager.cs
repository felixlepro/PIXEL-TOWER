using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class SwordIceManager : MonoBehaviour {

    public SwordIce sI;
    public Enemy enemy;
    public EnemyManager enemyM;
    private Vector3 vZero = new Vector3(0, 0, 0);
    private float flZero = 0f;
    int slowChances;
    int nbRand;
    int slowTimer;
    float slowMult;
    private void Start()
    {
        slowMult = sI.slowMultip; 
        slowChances = sI.chanceSlowProc;
        slowTimer = sI.slowTime;
        nbRand = Rand();
        Debug.Log(nbRand);
    }


    private void MightSlow()
    {
        if (Rand() <= slowChances)
        {
            float time = 0;

            while (time < slowTimer)
            {
                time += Time.deltaTime;
                PlsSlow(); 
            }

            PlsDontSlow();

        }
    }


    public void PlsSlow()
    {
        enemy.moveSpeed *= slowMult;
    }

    public void PlsDontSlow()
    {
        enemy.moveSpeed /= slowMult;
    }

    public int Rand()
    {
        int dabNbRand = Random.Range(0, 100);
        return dabNbRand;
    }

}

