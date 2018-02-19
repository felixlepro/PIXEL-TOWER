using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewinding : MonoBehaviour
{
    int temps = 0;
    bool isRewinding = false;
    List<PositionPlus> position;
    PositionPlus pet = new PositionPlus(transform.position, 0f);

    void Start()
    {
        position = new List<PositionPlus>();
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
            if (temps == 0)
            {
                Record();
                temps = 1;
            }
            else
            {
                temps = 0;
            }

        }
    }
    void Rewind()
    {
        if (position.Count > 0)
        {
           // transform.position = position[0].;
            position.RemoveAt(0);
        }

    }
    void Record()
    {
        if (position.Count > Mathf.Round(1f / Time.fixedDeltaTime))
            position.RemoveAt(position.Count - 1);
        //PositionPlus pet = new PositionPlus(transform.position, 0f);
        //position.Insert(0, );
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }
}

