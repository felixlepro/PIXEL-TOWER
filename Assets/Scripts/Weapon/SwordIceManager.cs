using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class SwordIceManager : SwordManager
{

    public int chanceSlowProc;
    public int slowTime;
    public float slowMultip;


    public EnemyManager enemy;
    private Vector3 vZero = new Vector3(0, 0, 0);
    private float flZero = 0f;
    int slowChances;
    int nbRand;
    int slowTimer;
    float slowMult;

    public SwordIceManager()
    {
        wColor = Color.blue;
        attackDamage = 20;
        slowTime = 3;
        slowMultip = 0.65f;
        chanceSlowProc = 48;
    }
    private void Start()
    {
        slowMult = slowMultip;
        slowChances = chanceSlowProc;
        slowTimer = slowTime;
        nbRand = Rand();
        Debug.Log(nbRand);
    }

    private void MightSlow()
    {
        if (Rand() <= slowChances)
        {
            float time = 0;
            PlsSlow();
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
        enemy.maxMoveSpeed *= slowMult;
    }

    public void PlsDontSlow()
    {
        enemy.maxMoveSpeed /= slowMult;
    }

    public int Rand()
    {
        int dabNbRand = Random.Range(0, 100);
        return dabNbRand;
    }

}

