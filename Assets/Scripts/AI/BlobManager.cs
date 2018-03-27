﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobManager : EnemyManager
{

    public float hpRatioLostOnAttack;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        currentSpeed = maxMoveSpeed;
        pathingUnit = GetComponent<Unit>();
        pathingUnit.speed = currentSpeed;

        chaseTarget = GetComponentInParent<PlayerTarget>().playerTarget;
        hp = maxHp;

        enemyRigidbody = GetComponent<Rigidbody2D>();
        controller = GetComponent<StateController>();

        anim = GetComponentInChildren<Animator>();
        // anim.runtimeAnimatorController = animator;

        spriteR = gameObject.transform.Find("EnemyGraphics").gameObject.GetComponentInChildren<SpriteRenderer>();
        spriteR.color = wColor;

        enemyCollider = GetComponentInChildren<Collider2D>();
        targetCollider = chaseTarget.GetComponents<Collider2D>();
        attacks[0].attackHitbox = transform.Find("AttackHitbox").gameObject.GetComponents<Collider2D>();
    }
    public override void TryAttack()
    {
       // Debug.Log("tryA");
        Attack(attacks[0]);
    }

    public override void AttackSuccessful()
    {
        Slow(0.9f, attackSpeed, true);
        hp -= Mathf.FloorToInt(maxHp * hpRatioLostOnAttack);
        StartCoroutine("RedOnly");
        VerifyDeath();
    }

    public override void Damaged()
    {

    }
    public override void gonnaDie()
    {

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
        DyingLeft,

        Idling,
        Moving,
        Dying
    }

    State state;




    public override void UpdateAnim()
    {
        UpdateAnimState();

    }

    void UpdateAnimState()
    {
        //Debug.Log(isAttacking);

        switch (state)
        {
            case State.Moving:
                {
                    if (Angle >= 0 && Angle <= 90) SetOrKeepState(State.MoveFR);
                    else if (Angle < 180 && Angle > 90) SetOrKeepState(State.MoveBR);
                    else if (Angle < 270 && Angle > 180) SetOrKeepState(State.MoveBL);
                    else if (Angle < 360 && Angle > 270) SetOrKeepState(State.MoveFL);
                }
                break;
            case State.Dying:
                {
                    if (Angle >= 180 && Angle <= 360)
                    {
                        SetOrKeepState(State.DyingLeft);
                    }
                    else if (Angle < 180 && Angle > 0)
                    {
                        SetOrKeepState(State.DyingRight);
                    }
                    break;
                }

            case State.Idling:
            default:
                {
                    //Debug.Log("animIdle");
                    SetOrKeepState(State.IdleFront);
                }
                break;

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

    public override bool CheckAttack()
    {
        return true;
    }

    public override void setAnimState(string newState)
    {
        switch (newState)
        {
            case "Moving": state = State.Moving; break;
            case "Attacking": state = State.Moving; break;
            case "Idling": state = State.Idling; break;
            case "Dying": state = State.Dying; break;
        }
    }
    public override string getAnimState()
    {
        switch (state)
        {
            case State.Moving: return "Moving";
            case State.Idling: return "Idling";
            case State.Dying: return "Dying";
        }
        return "Idling";
    }
}
