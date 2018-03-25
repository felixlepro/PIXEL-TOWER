using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : BossManager {

     State state;

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

        pathingUnit.targetPosition = chaseTarget.position;
    }
    private void Update()
    {
        pathingUnit.targetPosition = chaseTarget.position;
        if (isRooted) pathingUnit.speed = 0;
        else pathingUnit.speed = currentSpeed;
        //if (!isAttacking) anim.speed = currentSpeed / maxMoveSpeed;;
        UpdateAnim();
        spriteOrderInLayer();
        UpdatecurrentAttackCD();
    }
    public override bool TryAttack()
    {
        if (checkIfAttackIsReady())
        {
            float distance = Vector3.Distance(chaseTarget.transform.position, transform.position);
            if (distance < attacks[0].GetComponent<Attacks>().attackRange)
            {
                Instantiate(attacks[0].GetComponent<Attacks>().prefab, transform.position, Quaternion.identity);
                attacks[0].GetComponent <flameThrower>().Setup(chaseTarget.transform.position - transform.position, 1, 1);
                resetAttackCD();
                state = State.GrosCoup;
                return true;
            }
            else if (distance < attacks[1].GetComponent<Attacks>().attackRange)
            {
                Instantiate(attacks[1].GetComponent<Attacks>().prefab, transform.position, Quaternion.identity);
                attacks[1].GetComponent<MagicBall>().Setup(chaseTarget.transform.position - transform.position, 1, 1,1);
                resetAttackCD();
                state = State.AttackNormal;
                return true;
            }
        }
        return false;
    }
    public override void updateAnimState(string newState)
    {
        switch (newState)
        {
            case "Move": state = State.Moving; break;
            case "Idle": state = State.Idling; break;
            case "GrosCoup": state = State.GrosCoup; break;
            case "AttackNormal": state = State.AttackNormal; break;
            case "AttackSwing": state = State.AttackSwing; break;
            case "Summoning": state = State.Summoning; break;
        }
    }
    public override void gonnaDie()
    {
    }

    public override void Damaged()
    {
    }

    //Animations-----------------------------------------

    float stateStartTime;
    float timeInState
    {
        get { return Time.time - stateStartTime; }
    }

    const string IdleF = "Idle";
    const string IdleB = "IdleBack";

    const string WalkF = "Move";
    const string WalkB = "moveBack";

    const string Summon = "Summon";
    const string GrosCoup = "GrosCoup";
    const string AttackNormal = "Attack";
    const string AttackSwing = "BulletAttack";
    const string Teleport = "Teleport";
    const string DieRight = "DieRight";

    enum State
    {
        IdleFL,
        IdleFR,
        IdleBL,
        IdleBR,

        MoveFL,
        MoveFR,
        MoveBL,
        MoveBR,

        GrosCoupL,
        GrosCoupR,

        AttackNormalL,
        AttackNormalR,

        AttackSwingL,
        AttackSwingR,

        SummonL,
        SummonR,

        TeleportL,
        TeleportR,


        DyingR,
        DyingL,



        Idling,
        Moving,
        GrosCoup,
        AttackNormal,
        AttackSwing,
        Summoning,
        Teleporting,
        Dying
    }

    State stateAnim;

    public override void UpdateAnim()
    {

        UpdateAnimState();
    }

    void UpdateAnimState()
    {
        switch (state)
        {
            case State.Idling:
            case State.IdleFR:
            case State.IdleFL:
            case State.IdleBR:
            case State.IdleBL:
                if (Angle >= 0 && Angle <= 90) SetOrKeepState(State.IdleFR);
                else if (Angle < 180 && Angle > 90) SetOrKeepState(State.IdleBR);
                else if (Angle < 270 && Angle > 180) SetOrKeepState(State.IdleBL);
                else if (Angle < 360 && Angle > 270) SetOrKeepState(State.IdleFL);
                break;

            case State.Moving:
            case State.MoveBL:
            case State.MoveFL:
            case State.MoveFR:
            case State.MoveBR:
                if (Angle >= 0 && Angle <= 90) SetOrKeepState(State.MoveFR);
                else if (Angle < 180 && Angle > 90) SetOrKeepState(State.MoveBR);
                else if (Angle < 270 && Angle > 180) SetOrKeepState(State.MoveBL);
                else if (Angle < 360 && Angle > 270) SetOrKeepState(State.MoveFL);
                break;

            case State.GrosCoup:
            case State.GrosCoupL:
            case State.GrosCoupR:
                if (Angle >= 0 && Angle <= 180) SetOrKeepState(State.GrosCoupR);
                else if (Angle < 360 && Angle > 180) SetOrKeepState(State.GrosCoupL);
                break;

            case State.AttackNormal:
            case State.AttackNormalL:
            case State.AttackNormalR:
                if (Angle >= 0 && Angle <= 180) SetOrKeepState(State.AttackNormalR);
                else if (Angle < 360 && Angle > 180) SetOrKeepState(State.AttackNormalL);
                break;

            case State.AttackSwing:
            case State.AttackSwingL:
            case State.AttackSwingR:
                if (Angle >= 0 && Angle <= 180) SetOrKeepState(State.AttackSwingR);
                else if (Angle < 360 && Angle > 180) SetOrKeepState(State.AttackSwingL);
                break;

            case State.Summoning:
            case State.SummonL:
            case State.SummonR:
                if (Angle >= 0 && Angle <= 180) SetOrKeepState(State.SummonR );
                else if (Angle < 360 && Angle > 180) SetOrKeepState(State.SummonL);
                break;

            case State.Teleporting:
            case State.TeleportL:
            case State.TeleportR:
                if (Angle >= 0 && Angle <= 180) SetOrKeepState(State.TeleportR);
                else if (Angle < 360 && Angle > 180) SetOrKeepState(State.TeleportL);
                break;

            case State.Dying:
                if (Angle >= 0 && Angle <= 180) SetOrKeepState(State.DyingR);
                else if (Angle < 360 && Angle > 180) SetOrKeepState(State.DyingL);
                break;
        }
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
            case State.MoveBL:
                anim.Play(WalkB);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.MoveBR:
                anim.Play(WalkB);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.MoveFL:
                anim.Play(WalkF);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.MoveFR:
                anim.Play(WalkF);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;

            //Idle
            case State.IdleFR:
                    anim.Play(IdleF);
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    break;
            case State.IdleBR:
                anim.Play(IdleB);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.IdleFL:
                anim.Play(IdleF);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.IdleBL:
                anim.Play(IdleB);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;

                //Attacking
            case State.GrosCoupL:
                anim.Play(GrosCoup);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.GrosCoupR:
                anim.Play(GrosCoup);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.AttackNormalL:
                anim.Play(AttackNormal);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.AttackNormalR:
                anim.Play(AttackNormal);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.AttackSwingL:
                anim.Play(AttackSwing);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.AttackSwingR:
                anim.Play(AttackSwing);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.SummonL:
                anim.Play(Summon);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.SummonR:
                anim.Play(Summon);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.TeleportL:
                anim.Play(Teleport);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.TeleportR:
                anim.Play(Teleport);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;

            //Dying
            case State.DyingL:
                anim.Play(DieRight);
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.DyingR:
                anim.Play(DieRight);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
        }

        stateAnim = newState;
        stateStartTime = Time.time;
    }

  
}


