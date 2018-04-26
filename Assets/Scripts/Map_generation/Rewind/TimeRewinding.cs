using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewinding : MonoBehaviour
{
    public float nbSec;
    bool isRewinding = false;
    public static bool isFantoming = false;
    List<PositionPlus> positionRewind;
    public List<PositionPlus> positionCopie;
    public GameObject playerCopie;
    public Player scriptPlayer;
    public GameObject ancienWeapon;
    public GameObject currentWeapon;
    PlayerTarget pT;

    void Start()
    {
        pT = GameObject.Find("Enemies").GetComponent<PlayerTarget>();
        positionRewind = new List<PositionPlus>();
        positionCopie = new List<PositionPlus>();
        scriptPlayer = GameManager.instance.player.GetComponent<Player>();// GameObject.Find("Pilot").GetComponent<Player>();
        Debug.Log(scriptPlayer.currentWeaponIndex);
        ancienWeapon = currentWeapon = scriptPlayer.weaponList[scriptPlayer.currentWeaponIndex].gameObject;
     //   Debug.Log("cacacacacacaca");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.R))
            StopRewind();
        currentWeapon = scriptPlayer.weaponList[scriptPlayer.currentWeaponIndex].gameObject;
    }

    private void FixedUpdate()
    {
        if (isRewinding && !isFantoming )
        {
            Rewind();
        }
        else
        {
            Record();

        }
    }
    void Rewind()
    {
        if (positionRewind.Count > 2)
        {
            transform.position = positionRewind[0].position;
            positionCopie.Insert(0, positionRewind[0]);
            positionRewind.RemoveAt(0);
            positionCopie.Insert(0, positionRewind[0]);
            positionRewind.RemoveAt(0);
        }
        else
        {
            transform.position = positionRewind[0].position;
        }

    }
    void Record()
    {
        
        if (positionRewind.Count > Mathf.Round(nbSec  / Time.fixedDeltaTime))
            positionRewind.RemoveAt(positionRewind.Count - 1);

        if (currentWeapon == ancienWeapon )
        {
           
            positionRewind.Insert(0, new PositionPlus(transform.position, scriptPlayer.direction, Input.GetKey(KeyCode.Mouse0),null,null));
        }
        else
        {
            positionRewind.Insert(0, new PositionPlus(transform.position, scriptPlayer.direction, Input.GetKey(KeyCode.Mouse0),ancienWeapon, currentWeapon ));
            ancienWeapon = currentWeapon;
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        if (!isFantoming )
        {
            PlayerCopie fantome = Instantiate(playerCopie, transform.position, Quaternion.identity).GetComponent<PlayerCopie>();
            isFantoming = true;
            fantome.Initialize(positionCopie);
            
           
            pT.changeTarget(GameObject.Find("PilotCopie(Clone)").transform);
            
        }
        
       

        
    }
    public void FantomeMort()
    {
        isFantoming = false;
       pT.changeTarget(GameManager.instance.player.transform);
        
    }
}

