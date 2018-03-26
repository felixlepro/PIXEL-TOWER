
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public State currentState;
    public State remainState;

    [HideInInspector]
    public EnemyManager enemyManager;

    [HideInInspector]
    public float stateTimeElapsed;
    [HideInInspector]
    public float timeElapsed;



    [HideInInspector] public bool aiActive = false;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
    }
    void Update()
    {
        if (!aiActive)
            return;

        currentState.UpdateState(this);

    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

}

