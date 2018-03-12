﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobManager : EnemyManager {

     float attackCD = 1f;
    public Blob enemyBlob;

    private void Update()
    {
        UpdateAnim();
        spriteOrderInLayer();
        UpdatecurrentAttackCD();
       
    }
    public override void GetEnemy()
    {
        enemy = enemyBlob;
    }
    new public bool checkIfAttackIsReady()
    {
        if(enemy.attackSpeed - attackCD < 0)
        {
            attackCD = enemy.attackSpeed;
        }
        return (timeUntilNextAttack <= enemy.attackSpeed - attackCD);
    }
    public override void TryAttack()
    {
        if (checkIfAttackIsReady())
        {
            AIPathing.speed = enemy.moveSpeed;
            Debug.Log("rdy");
            Attack();

        }
        if (timeUntilNextAttack > 0)
        {
            Debug.Log((timeUntilNextAttack / enemy.attackSpeed));
            AIPathing.speed = enemy.moveSpeed * ((1 - (timeUntilNextAttack / enemy.attackSpeed)));
        }
        
    }
    public override void Damaged()
    {
        hp -= Mathf.FloorToInt(enemy.maxHp / enemyBlob.hpLostOnAttack);
        VerifyDeath();
    }

    //Animations-----------------------------------------

    float stateStartTime;
    float timeInState
    {
        get { return Time.time - stateStartTime; }
    }

    const string IdleF = "IdleF";

    const string MoveFL = "MoveFL";
    const string MoveFR = "MoveFR";
    const string MoveBL = "MoveBL";
    const string MoveBR = "MoveBR";

    const string DieRight = "DieRight";
    const string DieLeft = "DieLeft";

    enum State
    {
        IdleFront,

        MoveFL,
        MoveFR,
        MoveBL,
        MoveBR,

        DyingRight,
        DyingLeft

    }

    State state;
    Vector2 velocity;
    float horzInput;
    bool jumpJustPressed;
    bool jumpHeld;
    int airJumpsDone = 0;



    public void UpdateAnim()
    {

        UpdateAnimState();
    }

    void UpdateAnimState()
    {
        //Debug.Log(isAttacking);

        if (isWalking)
        {
            // Debug.Log("animawalk");
            if (Angle >= 0 && Angle <= 90) SetOrKeepState(State.MoveFR);
            else if (Angle < 180 && Angle > 90) SetOrKeepState(State.MoveBR);
            else if (Angle < 270 && Angle > 180) SetOrKeepState(State.MoveBL);
            else if (Angle < 360 && Angle > 270) SetOrKeepState(State.MoveFL);
        }
        else if (isDying)
        {
            if (Angle >= 180 && Angle <= 360)
            {
                SetOrKeepState(State.DyingLeft);
            }
            else if (Angle < 180 && Angle > 0)
            {
                SetOrKeepState(State.DyingRight);
            }

        }

        else
        {
            //Debug.Log("animIdle");
            SetOrKeepState(State.IdleFront);
        }

        // Debug.Log("animStay");
    }
    void SetOrKeepState(State state)
    {
        if (this.state == state) return;
        EnterState(state);
    }

    void EnterState(State state)
    {
        //ExitState();
        switch (state)
        {
            //Walking
            case State.MoveBL:
                anim.Play(MoveBL);
                break;
            case State.MoveBR:
                anim.Play(MoveBR);
                break;
            case State.MoveFL:
                anim.Play(MoveFL);
                break;
            case State.MoveFR:
                anim.Play(MoveFR);
                break;

            //Idle
            case State.IdleFront:
                anim.Play(IdleF);
                break;


            //Dying
            case State.DyingLeft:
                anim.Play(DieLeft);
                break;
            case State.DyingRight:
                anim.Play(DieRight);
                break;
        }

        this.state = state;
        stateStartTime = Time.time;
    }

}