using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewinding : MonoBehaviour
{
    int temps = 0;
    bool isRewinding = false;
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
        if (isRewinding)
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
        if (positionRewind.Count > Mathf.Round(1f / Time.fixedDeltaTime))
            positionRewind.RemoveAt(positionRewind.Count - 1);

        scriptPlayer = GameObject.Find("Pilot").GetComponent<Player>();
        positionRewind.Insert(0, new PositionPlus(transform.position, scriptPlayer.direction));
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;

        PlayerCopie fantome = Instantiate(playerCopie, transform.position, Quaternion.identity).GetComponent<PlayerCopie>();
        
        fantome.Initialize(positionCopie);

        
    }
}

