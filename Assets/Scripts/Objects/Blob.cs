using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Blob")]
public class Blob : Enemy {

    Blob()
    {
        hp = 100;
    }


    public override void startAttack()
    {

    }
    public override void mainAttack()
    {
        if (controller.CheckIfCountDownElapsed(attackSpeed))
        {
            foreach (Collider2D pc in controller.targetCollider)
            {
                if (controller.attackHitbox.IsTouching(pc))
                {
                    // Debug.Log("collided");
                    pc.gameObject.GetComponent<Player>().RecevoirDegats(attackDamage, pc.gameObject.transform.position - controller.transform.position, 1f);
                    break;
                }

            }
        }
    
    }
    public override void endAttack()
    {

    }


    //Animations-----------------------------------------

    float stateStartTime;
    float timeInState
    {
        get { return Time.time - stateStartTime; }
    }

    const string IdleF = "KIdleFront";

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



    public override void UpdateAnim(StateController c)
    {
        if (controller == null)
        {
            controller = c;
        }
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
            case State.MoveBL :
                controller.anim.Play(MoveBL );
                break;
            case State.MoveBR :
                controller.anim.Play(MoveBR );
                break;
            case State.MoveFL :
                controller.anim.Play(MoveFL );
                break;
            case State.MoveFR :
                controller.anim.Play(MoveFR );
                break;

            //Idle
            case State.IdleFront:
                controller.anim.Play(IdleF);
                break;


            //Dying
            case State.DyingLeft:
                controller.anim.Play(DieLeft);
                break;
            case State.DyingRight:
                controller.anim.Play(DieRight);
                break;
        }

        this.state = state;
        stateStartTime = Time.time;
    }

}
