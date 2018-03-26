using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightManager : EnemyManager {

    public float attackChargeTime;

   // GameObject attackColliderObject;
    float colorAlpha = 0;
    const float colorAlphaMax = 1f;
    //bool attackDone = false;

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
    private void Update()
    {
        if (isRooted) pathingUnit.speed = 0;
        else pathingUnit.speed = currentSpeed;
        if(state != State.Attacking)  anim.speed = currentSpeed / maxMoveSpeed;
        UpdateAnim();
        spriteOrderInLayer();
        UpdatecurrentAttackCD();
    }
    public override void TryAttack()
    {
        isRooted = true;
        pathingUnit.disablePathing();
        getAngleTarget();

        anim.speed = 0.7f / attackChargeTime;
        //Invoke("endAttack", attacks[0].GetComponent<KnightAttack>().attackChargeTime * 1.5f);
        //Instantiate(attacks[0].GetComponent<Attacks>().prefab, transform.position, Quaternion.identity);
        //attacks[0].GetComponent<KnightAttack>().Setup(chaseTarget.transform.position - transform.position, 1, 1);

        attacks[0].attackHitbox[0].gameObject.transform.localRotation = Quaternion.Euler(0, 0, Angle);
        StartCoroutine(AttackFade());
    }

    IEnumerator AttackFade()
    {
        float time = 0;

        while (time < attackChargeTime)
        {
            time += Time.deltaTime;
            colorAlpha = colorAlphaMax * (1 - (1 - (time / attackChargeTime)) * (1 - (time / attackChargeTime)) * (1 - (time / attackChargeTime)));
            attacks[0].attackHitbox[0].GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, colorAlpha);
            yield return null;

        }

        Attack(attacks[0]);

        while (time > 0)
        {
            time -= Time.deltaTime * 2;
            colorAlpha = colorAlphaMax * time / attackChargeTime;
            attacks[0].attackHitbox[0].GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, colorAlpha);
            yield return null;
        }

        endAttack();

    }
    public override bool CheckAttack()
    {
        return true;
    }
    public override void Damaged()
    {
        if (gettingKnockedBackAmount !=0 && state != State.Attacking)
        {
            StopCoroutine("AttackFade");
            endAttack();
        }
    }
    
    public void endAttack()
    {
        isRooted = false;
        pathingUnit.enablePathing();
        anim.speed = currentSpeed / maxMoveSpeed;
        //controller.enemyManager.isWalking = false; 

    }
    public override void AttackSuccessful()
    {
    }
    public override void gonnaDie()
    {
        StopCoroutine(AttackFade());
        attacks[0].attackHitbox[0].GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 0, 0, 0);
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
        DyingLeft,

        Idling,
        Moving,
        Attacking,
        Dying
    }

    State state;
    State stateAnim;
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
        switch (state)
        {
            //if (isAttacking)
            case State.Attacking:
                if (!(anim.GetCurrentAnimatorStateInfo(0).IsName(AttackS) || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackSE)
                   || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackE) || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackNE)
                   || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackN) || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackNW)
                   || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackW) || anim.GetCurrentAnimatorStateInfo(0).IsName(AttackSW)))
                {
                    // Debug.Log("animattack");
                    if (Angle <= 31 || Angle >= 327) SetOrKeepState(State.AttackingS);
                    else if (Angle >= 31 && Angle <= 57) SetOrKeepState(State.AttackingSE);
                    else if (Angle >= 57 && Angle <= 113) SetOrKeepState(State.AttackingE);
                    else if (Angle >= 113 && Angle <= 148) SetOrKeepState(State.AttackingNE);
                    else if (Angle >= 148 && Angle <= 212) SetOrKeepState(State.AttackingN);
                    else if (Angle >= 212 && Angle <= 239) SetOrKeepState(State.AttackingNW);
                    else if (Angle >= 239 && Angle <= 302) SetOrKeepState(State.AttackingW);
                    else if (Angle >= 302 && Angle <= 327) SetOrKeepState(State.AttackingSW);
                }break;

            //else if (isWalking)
            case State.Moving:
                {
                    // Debug.Log("animawalk");
                    if (Angle >= 316 || Angle <= 44) SetOrKeepState(State.WalkingFront);
                    else if (Angle < 314 && Angle > 226) SetOrKeepState(State.WalkingLeft);
                    else if (Angle < 224 && Angle > 136) SetOrKeepState(State.WalkingBack);
                    else if (Angle < 134 && Angle > 46) SetOrKeepState(State.WalkingRight);
                } break;
            //else if (isDying)
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

                }break;

            // else
            case State.Idling:
            default:
                {
                    //Debug.Log("animIdle");
                    if (Angle >= 315.5 || Angle <= 44.5) SetOrKeepState(State.IdleFront);
                    else if (Angle <= 314.5 && Angle >= 225.5) SetOrKeepState(State.IdleLeft);
                    else if (Angle <= 224.5 && Angle >= 135.5) SetOrKeepState(State.IdleBack);
                    else if (Angle <= 134.5 && Angle >= 45.5) SetOrKeepState(State.IdleRight);
                }break;
        }
        // Debug.Log("animStay");
    }
    void SetOrKeepState(State newState)
    {
        if (stateAnim == newState) return;
        EnterState(newState);
    }

    void EnterState(State newState)
    {
        //ExitState();
        switch (newState)
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

        stateAnim = state;
        stateStartTime = Time.time;
    }

    public override void setAnimState(string newState)
    {
        switch (newState)
        {
            case "Moving": state = State.Moving; break;
            case "Idling": state = State.Idling; break;
            case "Attacking": state = State.Attacking; break;
            case "Dying": state = State.Dying; break;
        }
    }
    public override string getAnimState()
    {
        switch (state)
        {
            case State.Moving: return "Moving";
            case State.Idling: return "Idling";
            case State.Attacking: return "Attacking";
            case State.Dying: return "Dying";
        }
        return "Idling";
    }
}

