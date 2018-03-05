using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour {

    public GameObject chaseTarget;
    public EnemyManager[] enemyGM;

    public void changeChaseTarget()
    {
        enemyGM = GetComponentsInChildren<EnemyManager>();
        foreach (EnemyManager em in enemyGM)
        {
            em.chaseTarget = chaseTarget;
        }
    }
}
