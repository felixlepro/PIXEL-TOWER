using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightManager : EnemyManager {

    public float attackChargeTime;

    GameObject attackColliderObject;
    float colorAlpha = 0;
    const float colorAlphaMax = 1f;
    bool attackDone = false;

    public override void TryAttack()
    {
        pathingUnit.speed = 0;
        pathingUnit.disablePathing();
        getAngleTarget();
        attackHitbox[0].gameObject.transform.localRotation = Quaternion.Euler(0, 0, Angle);
        attackDone = false;
        StartCoroutine("AttackFade", attackChargeTime );
    }

    IEnumerator AttackFade()
    {
        float time = 0;
        anim.speed = 0.7f/ attackChargeTime;
        while (time < attackChargeTime)
        {
            time += Time.deltaTime;
            colorAlpha = colorAlphaMax * (1 - (1 - (time / attackChargeTime)) * (1 - (time / attackChargeTime)) * (1 - (time / attackChargeTime)));
            attackHitbox[0].GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, colorAlpha);
            yield return null;
            
        }

        Attack();

        while (time > 0)
        {
            time -= Time.deltaTime * 2;
            colorAlpha = colorAlphaMax * time / attackChargeTime;
            attackHitbox[0].GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, colorAlpha);
            yield return null;
        }

        endAttack();
       
    }
    public override void Damaged()
    {
        if (gettingKnockedBackAmount !=0 && isAttacking == true)
        {
            StopCoroutine("AttackFade");
            endAttack();
        }
    }
    
    public void endAttack()
    {
        pathingUnit.speed = currentSpeed;
        pathingUnit.enablePathing();
        attackHitbox[0].GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, 0);
        controller.enemyManager.isAttacking = false;
        anim.speed = 1;
        //controller.enemyManager.isWalking = false; 

    }
    public override void AttackSuccessful()
    {
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



    public override void UpdateAnim()
    {

        UpdateAnimState();
    }

    void UpdateAnimState()
    {
        //Debug.Log(isAttacking);
        if (isAttacking)
        {
            if (!(anim.GetCurrentAnimatorStateInfo(0).IsName(AttackS) || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackSE)
               || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackE) || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackNE)
               || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackN) || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackNW)
               || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackW) || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackSW)))
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
                anim.Play(WalkL);
                break;
            case State.WalkingFront:
                anim.Play(WalkF);
                break;
            case State.WalkingBack:
                anim.Play(WalkB);
                break;
            case State.WalkingRight:
                anim.Play(WalkR);
                break;

            //Idle
            case State.IdleFront:
                anim.Play(IdleF);
                break;
            case State.IdleRight:
                anim.Play(IdleR);
                break;
            case State.IdleBack:
                anim.Play(IdleB);
                break;
            case State.IdleLeft:
                anim.Play(IdleL);
                break;

            //Attacking
            case State.AttackingS:
                anim.Play(AttackS);

                break;
            case State.AttackingSE:
                anim.Play(AttackSE);
                break;
            case State.AttackingE:
                anim.Play(AttackE);
                break;
            case State.AttackingNE:
                anim.Play(AttackNE);
                break;
            case State.AttackingN:
                anim.Play(AttackN);
                break;
            case State.AttackingNW:
                anim.Play(AttackNW);
                break;
            case State.AttackingW:
                anim.Play(AttackW);
                break;
            case State.AttackingSW:
                anim.Play(AttackSW);
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
