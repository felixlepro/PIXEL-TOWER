using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Knight")]
public class Knight : Enemy
{
    GameObject attackColliderObject;
   const float attackChargeTime = 0.7f;
    float colorAlpha = 0;
    const float colorAlphaMax = 1f;
    bool attackDone = false;
    float time = 0;

    Knight()
    {
        hp = 100;
        
    }

    public override void startAttack()
    {
        controller.AIPathing.maxSpeed = 0;
        //Debug.Log("StartAttack");
        controller.getAngleTarget();
        controller.attackHitbox.gameObject.transform.localRotation = Quaternion.Euler(0, 0, Angle);
        attackDone = false;
    }

    public override void mainAttack()
    {
        // Debug.Log(attackDone);
        if (!attackDone)
        {
            time += Time.deltaTime;
            colorAlpha = colorAlphaMax * (1 -(1-(time / attackChargeTime)) * (1-(time / attackChargeTime))* (1 - (time / attackChargeTime)));
            controller.attackHitbox.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, colorAlpha);

            if (controller.CheckIfCountDownElapsed2(attackChargeTime))
            {
               // Debug.Log("MainAttack");
                foreach (Collider2D pc in controller.targetCollider)
                {
                    if (controller.attackHitbox.IsTouching(pc))
                    {
                        // Debug.Log("collided");
                        pc.gameObject.GetComponent<Player>().RecevoirDegats(attackDamage, pc.gameObject.transform.position - controller.transform.position, 0.5f);
                        break;
                    }

                }
                attackDone = true;
             //   Debug.Log(attackDone);
            }
        }
        else
        {
            if (time > 0)
            {
                time -= Time.deltaTime * 2;
                colorAlpha = colorAlphaMax * time / attackChargeTime;
                controller.attackHitbox.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, colorAlpha);
            }
            else
            {
                controller.attackHitbox.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, 0);
                time = 0;
            }
        }
         
    }
    public override void endAttack()
    {
        controller.attackHitbox.GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, 0);
        time = 0;
    }

    //Animations-----------------------------------------

    float stateStartTime;
    float timeInState
    {
        get { return Time.time - stateStartTime; }
    }

    const string IdleF = "KIdleFront";
    const string IdleB = "KIdleBack";
    const string IdleL = "KIdleLeft";
    const string IdleR = "KIdleRight";

    const string WalkF = "KWalkFront";
    const string WalkB = "KWalkBack";
    const string WalkL = "KWalkLeft";
    const string WalkR = "KWalkRight";

    const string AttackS = "KAttackS";
    const string AttackSE = "KAttackSE";
    const string AttackE = "KAttackE";
    const string AttackNE = "KAttackNE";
    const string AttackN = "KAttackN";
    const string AttackNW = "KAttackNW";
    const string AttackW = "KAttackW";
    const string AttackSW = "KAttackSW";
    const string DieRight = "KDieRight";
    const string DieLeft = "KDieLeft";

    enum State
    {
        IdleFront,
        IdleBack,
        IdleLeft,
        IdleRight,

        WalkingFront,
        WalkingBack,
        WalkingLeft,
        WalkingRight,

        AttackingS,
        AttackingSE,
        AttackingE,
        AttackingNE,
        AttackingN,
        AttackingNW,
        AttackingW,
        AttackingSW,
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
        if(controller == null)
        {
            controller = c;
        }
        UpdateAnimState();
        ContinueState();
    }

    void UpdateAnimState()
    {
        //Debug.Log(isAttacking);
        if (isAttacking)
        {
            if (!(controller.anim.GetCurrentAnimatorStateInfo(0).IsName(AttackS) || controller.anim.GetCurrentAnimatorStateInfo(0).IsName(AttackSE)
               || controller.anim.GetCurrentAnimatorStateInfo(0).IsName(AttackE) || controller.anim.GetCurrentAnimatorStateInfo(0).IsName(AttackNE)
               || controller.anim.GetCurrentAnimatorStateInfo(0).IsName(AttackN) || controller.anim.GetCurrentAnimatorStateInfo(0).IsName(AttackNW)
               || controller.anim.GetCurrentAnimatorStateInfo(0).IsName(AttackW) || controller.anim.GetCurrentAnimatorStateInfo(0).IsName(AttackSW)))
            {
                // Debug.Log("animattack");
                if (Angle <= 24 || Angle >= 334) SetOrKeepState(State.AttackingS);
                else if (Angle >= 24 && Angle <= 64) SetOrKeepState(State.AttackingSE);
                else if (Angle >= 64 && Angle <= 116) SetOrKeepState(State.AttackingE);
                else if (Angle >= 116 && Angle <= 155) SetOrKeepState(State.AttackingNE);
                else if (Angle >= 155 && Angle <= 205) SetOrKeepState(State.AttackingN);
                else if (Angle >= 205 && Angle <= 246) SetOrKeepState(State.AttackingNW);
                else if (Angle >= 246 && Angle <= 295) SetOrKeepState(State.AttackingW);
                else if (Angle >= 295 && Angle <= 334) SetOrKeepState(State.AttackingSW);
            }
        }
        else if (isWalking)
        {
            // Debug.Log("animawalk");
            if (Angle >= 316 || Angle <= 44) SetOrKeepState(State.WalkingFront);
            else if (Angle < 314 && Angle > 226) SetOrKeepState(State.WalkingLeft);
            else if (Angle < 224 && Angle > 136) SetOrKeepState(State.WalkingBack);
            else if (Angle < 134 && Angle > 46) SetOrKeepState(State.WalkingRight);
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
            if (Angle >= 315.5 || Angle <= 44.5) SetOrKeepState(State.IdleFront);
            else if (Angle <= 314.5 && Angle >= 225.5) SetOrKeepState(State.IdleLeft);
            else if (Angle <= 224.5 && Angle >= 135.5) SetOrKeepState(State.IdleBack);
            else if (Angle <= 134.5 && Angle >= 45.5) SetOrKeepState(State.IdleRight);
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
            case State.WalkingLeft:
                controller.anim.Play(WalkL);
                break;
            case State.WalkingFront:
                controller.anim.Play(WalkF);
                break;
            case State.WalkingBack:
                controller.anim.Play(WalkB);
                break;
            case State.WalkingRight:
                controller.anim.Play(WalkR);
                break;

                //Idle
            case State.IdleFront :
                controller.anim.Play(IdleF);
                break;
            case State.IdleRight :
                controller.anim.Play(IdleR);
                break;
            case State.IdleBack:
                controller.anim.Play(IdleB);
                break;
            case State.IdleLeft:
                controller.anim.Play(IdleL);
                break;

                //Attacking
            case State.AttackingS :
                controller.anim.Play(AttackS);

                break;
            case State.AttackingSE :
                controller.anim.Play(AttackSE);
                break;
            case State.AttackingE :
                controller.anim.Play(AttackE);
                break;
            case State.AttackingNE:
                controller.anim.Play(AttackNE);
                break;
            case State.AttackingN:
                controller.anim.Play(AttackN);
                break;
            case State.AttackingNW:
                controller.anim.Play(AttackNW);
                break;
            case State.AttackingW:
                controller.anim.Play(AttackW);
                break;
            case State.AttackingSW:
                controller.anim.Play(AttackSW);
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

    //void ExitState()
    //{
    //}

    void ContinueState()
    {
        switch (state)
        {

            //case State.RunningLeft:
            //case State.RunningRight:
            //    if (!RunOrJump()) EnterState(State.Idle);
            //    break;

            
        }
    }
}

    