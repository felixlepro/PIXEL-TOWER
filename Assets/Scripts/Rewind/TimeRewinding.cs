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
    void Start()
    {
        positionRewind = new List<PositionPlus>();
        positionCopie = new List<PositionPlus>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            StartRewind();
        if (Input.GetKeyUp(KeyCode.R))
            StopRewind();
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

        scriptPlayer = GameObject.Find("Pilot").GetComponent<Player>();
        positionRewind.Insert(0, new PositionPlus(transform.position, scriptPlayer.direction, Input.GetKeyDown(KeyCode.Mouse0)));
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
            
           
            GameObject.Find("Enemies").GetComponent<PlayerTarget>().changeTarget(GameObject.Find("PilotCopie(Clone)").transform);
            
        }
        
       

        
    }
    public void FantomeMort()
    {
        isFantoming = false;
        GameObject.Find("Enemies").GetComponent<PlayerTarget>().changeTarget(GameObject.Find("Pilot").transform);
       
    }
}

