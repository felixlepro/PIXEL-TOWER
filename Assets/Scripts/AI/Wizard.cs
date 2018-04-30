using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : EnemyManager {

     State state;
    public int nbrBoule;
    Transform graphics;
    bool dying = false;

    protected override void OnStart()
    {
       setAnimState("Moving");
        graphics = transform.Find("EnemyGraphics");
    }
    private void Update()
    {
        pathingUnit.targetPosition = chaseTarget.position;
        if (isRooted) pathingUnit.speed = 0;
        else pathingUnit.speed = currentSpeed;
        //if (!isAttacking) anim.speed = currentSpeed / maxMoveSpeed;;
        UpdateAnim();
        UpdatecurrentAttackCD();
    }
    public override bool CheckAttack()
    {
        if (checkIfAttackIsReady())
        {
            float distance = Vector3.Distance(chaseTarget.transform.position, transform.position);
            if (attacks[0].checkIfAttackIsReady() && distance < attacks[0].attackRange)
            {
                StartCoroutine(Root(1));
                state = State.AttackNormal;
                resetAttackCD();
                attacks[0].resetAttackCD();               
                Invoke("doAttackFlameThrower", 1);
                return true;
            }
            else if (attacks[2].checkIfAttackIsReady() && distance > attacks[2].attackRange)
            {
                StartCoroutine(Root(1));
                state = State.Summoning;
                resetAttackCD();
                attacks[2].resetAttackCD();              
                GameObject fT = Instantiate(attacks[2].prefab, playerMovementPrediction(0.5f,1.25f), Quaternion.identity);
                fT.GetComponent<CerclePuissant>().Setup(Vector3.zero, attacks[2].attackDamage, attacks[2].maxKnockBackAmount, attacks[2].immuneTime, attacks[2].speed, 
                                                                      attacks[2].burnChance, attacks[2].burnDamage, attacks[2].burnDuration, 
                                                                      attacks[2].slowChance, attacks[2].slowAmount, attacks[2].slowDuration);
                return true;
            }
            else if (attacks[1].checkIfAttackIsReady() && distance < attacks[1].attackRange)
            {
                StartCoroutine(Root(1));
                state = State.AttackSwing;
                resetAttackCD();
                attacks[1].resetAttackCD();             
                for (float i = 0; i < nbrBoule; i++)
                {
                    float delay = i/4f;
                    Invoke("doAttackFireBall", 0.5f + delay);
                }
                return true;
            }
        }
        return false;
    }
    void doAttackFlameThrower()
    {
        if (!dying)
        {
            GameObject fT = Instantiate(attacks[0].prefab, transform.position, Quaternion.identity);
            fT.GetComponent<flameThrower>().Setup(chaseTarget.transform.position - transform.position + Vector3.up / 2, attacks[0].attackDamage, attacks[0].maxKnockBackAmount, attacks[0].immuneTime, attacks[0].speed,
                                                                          attacks[0].burnChance, attacks[0].burnDamage, attacks[0].burnDuration,
                                                                          attacks[0].slowChance, attacks[0].slowAmount, attacks[0].slowDuration);
        }
    }
    void doAttackFireBall()
    {
        if (!dying)
        {
            GameObject fT = Instantiate(attacks[1].prefab, transform.position + Vector3.up / 3, Quaternion.identity);
            Vector3 dir = playerMovementPrediction((chaseTarget.position - transform.position).magnitude / attacks[1].speed, 1) - transform.position + Vector3.up / 2;
            fT.GetComponent<MagicBall>().Setup(dir, attacks[1].attackDamage, attacks[1].maxKnockBackAmount, attacks[1].attackRange, attacks[1].immuneTime, attacks[1].speed,
                                                                            attacks[1].burnChance, attacks[1].burnDamage, attacks[1].burnDuration,
                                                                          attacks[1].slowChance, attacks[1].slowAmount, attacks[1].slowDuration);
        }
    }
    public override void setAnimState(string newState)
    {
        //if (state != State.Dying)
      //  {
            switch (newState)
            {
                case "Moving": state = State.Moving; break;
                case "Idling": state = State.Idling; break;
                case "GrosCoup": state = State.GrosCoup; break;
                case "AttackNormal": state = State.AttackNormal; break;
                case "AttackSwing": state = State.AttackSwing; break;
                case "Summoning": state = State.Summoning; break;
                case "Dying": state = State.Dying; break;
            }
       // }
    }
    public override string getAnimState()
    {
        switch (state)
        {
            case State.Moving: return "Moving";
            case State.Idling: return "Idling";
            case State.GrosCoup: return "GrosCoup";
            case State.AttackNormal: return "AttackNormal";
            case State.AttackSwing: return "AttackSwing";
            case State.Summoning:return "Summoning";
            case State.Dying: return "Dying";
        }
        return "Idling";
    }
    public override void gonnaDie()
    {
        dying = true;
  
    }
    protected override void DropItems()
    {
        if (GameManager.instance.boardBoss.nbrEnemy-- == 1) { 
        nbrCoins = Random.Range(Mathf.RoundToInt(nbrCoins / 2), Mathf.RoundToInt(nbrCoins * 1.5f));
        DropManager.DropCoin(transform.position, nbrCoins);
        DropManager.DropKey(transform.position);
        if (Random.value * 100 < weaponDropChance)
        {
            DropManager.DropRandomWeapon(transform.position, true);
        }
    }
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
        if (updateAnim)
        {
            UpdateAnimState();

        }

    }
    public void UpdateAnimState()
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
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.MoveBR:
                anim.Play(WalkB);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.MoveFL:
                anim.Play(WalkF);
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.MoveFR:
                anim.Play(WalkF);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;

            //Idle
            case State.IdleFR:
                    anim.Play(IdleF);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    break;
            case State.IdleBR:
                anim.Play(IdleB);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.IdleFL:
                anim.Play(IdleF);
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.IdleBL:
                anim.Play(IdleB);
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;

                //Attacking
            case State.GrosCoupL:
                anim.Play(GrosCoup);
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.GrosCoupR:
                anim.Play(GrosCoup);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.AttackNormalL:
                anim.Play(AttackNormal);
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.AttackNormalR:
                anim.Play(AttackNormal);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.AttackSwingL:
                anim.Play(AttackSwing);
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.AttackSwingR:
                anim.Play(AttackSwing);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.SummonL:
                anim.Play(Summon);
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.SummonR:
                anim.Play(Summon);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.TeleportL:
                anim.Play(Teleport);
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.TeleportR:
                anim.Play(Teleport);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;

            //Dying
            case State.DyingL:
                anim.Play(DieRight);
                graphics.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
            case State.DyingR:
                anim.Play(DieRight);
                graphics.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                break;
        }

        stateAnim = newState;
        stateStartTime = Time.time;
    }

    public override void TryAttack()
    {
        throw new System.NotImplementedException();
    }

    public override void AttackSuccessful()
    {
        throw new System.NotImplementedException();
    }
}


