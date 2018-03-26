using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyManager : MonoBehaviour {
    public float Angle;
    public bool isWalking;
    public float speed;
    public float maxSpeed;
    public Transform player;
    public bool chasing;
    public float chaseSpeed;
    Transform   target;
    float dMin;
    int iMin;
    Vector2 distance;
    GameObject[] listCoinGO;
    List<Vector3 > listCoinD;
    Animator anim;
    SpriteRenderer spriteR;
    Player playerS;
    Unit unity;

	void Start () {
        anim = GetComponent<Animator>();
        unity = GetComponent<Unit>();
        spriteR = GetComponent<SpriteRenderer>();
        playerS = GameObject.Find("Pilot").GetComponent<Player>();
        target = player;
	}
	
	// Update is called once per frame
	void Update () {
        getAnglePath();
        listCoinD = new List<Vector3 >();
        if (!chasing) ChaseCoins(); 
        SetSpeed();
        unity.targetPosition = target.position;
        UpdateAnim();
        spriteOrderInLayer();
        
	}
    public void SetSpeed()
    {
        if (chasing )
        {
            speed = chaseSpeed ;
        }
        else
        {
            distance = (player.position  - transform.position);

            if (distance.magnitude > 1.5f)
            {
                speed = Mathf.Clamp(Mathf.Pow(2,distance.magnitude),0,maxSpeed );
                isWalking = true;
            }
            else
            {
                isWalking = false;
                speed = 0;
            }
        }
        unity.speed = speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coin")
        {
            playerS.gainCoin();
            GameObject.Find("GameManager").GetComponent<GameManager>().PlaySound(GameObject.Find("GameManager").GetComponent<GameManager>().coinSound );
            chasing = false;
            Destroy(other.gameObject);
        }
    }
    public void ChaseCoins()
    {
        
        listCoinGO = GameObject.FindGameObjectsWithTag("Coin");
        if (listCoinGO.Length != 0)
        {
            chasing = true;
            dMin = 100;
            for (int i = 0; i < listCoinGO.Length; i++)
            {
                listCoinD.Add(listCoinGO[i].transform.position - player.transform.position);
                if (listCoinD[0].magnitude < dMin)
                {
                    dMin = listCoinD[i].magnitude;
                    iMin = i;
                }
            }
            target = listCoinGO[iMin].transform;
        }
        else target = player;
        

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
