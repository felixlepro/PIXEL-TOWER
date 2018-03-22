using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyManager : MonoBehaviour {

    public float Angle;
    public bool isWalking;
    public float speed;
    public Transform player;
    Animator anim;
    SpriteRenderer spriteR;
    Unit unity;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        unity = GetComponent<Unit>();
        spriteR = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        getAnglePath();
        unity.targetPosition = player.position ;
        unity.speed = speed;
        UpdateAnim();
        spriteOrderInLayer();
        isWalking = unity.IsWalking();
	}
    public void spriteOrderInLayer()
    {
        if (player.position.y <= transform.position.y)
        {
            spriteR.sortingOrder = -2;
        }
        else spriteR.sortingOrder = 2;
    }
    public void getAnglePath()
    {
        Vector2 direction = unity.direction;
        if (direction != Vector2.zero)
        {
            float angle = Vector2.Angle(direction, new Vector2(0, -1));
            if (direction.x < 0) angle = 360 - angle;
            Angle = angle;
        }
    }
    float stateStartTime;
    float timeInState
    {
        get { return Time.time - stateStartTime; }
    }

    const string IdleL = "Piggy_idle_left";
    const string IdleR = "Piggy_idle_right";

    const string MoveL = "Piggy_move_left";
    const string MoveR = "Piggy_move_right";
    

    enum State
    {
        IdleL,
        IdleR,

        MoveL,
        MoveR,
        

    }

    State state;

    public void UpdateAnim()
    {
        UpdateAnimState();

    }

    void UpdateAnimState()
    {
        if (isWalking)
        {
            if (Angle >= 0 && Angle <= 180) SetOrKeepState(State.MoveR);
            else if (Angle < 360 && Angle > 180) SetOrKeepState(State.MoveL);
        }
        else
        {
            if (Angle >= 0 && Angle <= 180) SetOrKeepState(State.IdleR);
            else if (Angle < 360 && Angle > 180) SetOrKeepState(State.IdleL);
        }
        
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
            case State.MoveL:
                anim.Play(MoveL);
                break;
            case State.MoveR:
                anim.Play(MoveR);
                break;
            

            //Idle
            case State.IdleL:
                anim.Play(IdleL);
                break;
            case State.IdleR:
                anim.Play(IdleR);
                break;

        }

        this.state = state;
        stateStartTime = Time.time;
    }
}
