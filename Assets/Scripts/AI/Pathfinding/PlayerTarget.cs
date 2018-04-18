using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour {

    [HideInInspector] public Transform playerTarget;

    private void Awake()
    {
        playerTarget = GameManager.instance.player.transform;
    }
    public void changeTarget(Transform newTarget)
    {
        playerTarget = newTarget;

        EnemyManager[] enemyManager = GetComponentsInChildren<EnemyManager>();
        foreach (EnemyManager em in enemyManager)
        {
            em.chaseTarget = playerTarget;
        }
    }
}
