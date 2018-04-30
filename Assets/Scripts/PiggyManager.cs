using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyManager : MonoBehaviour {
    float Angle;
    bool isWalking;
    float speed;
    public float maxSpeed;
    public Transform player;
    public bool chasing;
    public float chaseSpeed;
    Transform   target;
    float dMin;
    int iMin;
    Vector2 distance;
    //GameObject[] listCoinGO;
    [HideInInspector] public List<GameObject> coinList;

   // List<Vector3 > listCoinD;
    Animator anim;
  //  SpriteRenderer spriteR;
    Player playerS;
    Unit unity;


	void Start () {
        coinList.Clear();
        anim = GetComponent<Animator>();
        unity = GetComponent<Unit>();
       // spriteR = GetComponent<SpriteRenderer>();
        player = GameManager.instance.player.transform;
        playerS = GameManager.instance.player.GetComponent<Player>();
        // GameObject.Find("Pilot Copie").GetComponent<Player>();
        target = player;
	}
	
	// Update is called once per frame
	void Update () {
        getAnglePath();
 //       listCoinD = new List<Vector3 >();
        ChaseCoins(); 
        SetSpeed();
        unity.targetPosition = target.position;
        UpdateAnim();
	}
    public void SetSpeed()
    {
        if (chasing)
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
            coinList.Remove(other.gameObject);
            coinList.TrimExcess();
            playerS.gainCoin();
            GameManager.instance.PlaySound(GameManager.instance.coinSound );
            chasing = false;
            Destroy(other.gameObject);
        }
    }
    public void ChaseCoins()
    {
            //listCoinGO = GameObject.FindGameObjectsWithTag("Coin");
            if (coinList.Count != 0)
            {
                chasing = true;
                dMin = 10;
                iMin = -1;
                for (int i = 0; i < coinList.Count; i++)
                {

                    if ((coinList[i].transform.position - player.transform.position).magnitude < dMin)
                    {
                        dMin = (coinList[i].transform.position - player.transform.position).magnitude;
                        iMin = i;
                    }
                }
                if (iMin != -1)
                {
                    target = coinList[iMin].transform;
//                    Debug.Log("ok");
                }
                else target = player;
            }
            else target = player;       
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
