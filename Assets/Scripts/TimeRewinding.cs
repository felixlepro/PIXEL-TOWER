using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewinding : MonoBehaviour
{
    int temps = 0;
    bool isRewinding = false;
    List<PositionPlus> positionRewind;
    List<PositionPlus> positionCopie;
    public GameObject playerCopie;
    public PlayerCopie scriptCopie;
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
        if (positionRewind.Count > 0)
        {
            transform.position = positionRewind[0].position ;
            positionCopie.Insert(0, positionRewind[0]);
            positionRewind.RemoveAt(0);
        }

    }
    void Record()
    {
        if (positionRewind.Count > Mathf.Round(1f / Time.fixedDeltaTime))
            positionRewind.RemoveAt(positionRewind.Count - 1);

        positionRewind.Insert(0, new PositionPlus(transform.position, new Vector3 (1,1)));
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
        Instantiate(playerCopie, transform.position, Quaternion.identity);
        //marche pas
        scriptCopie = GetComponent<PlayerCopie>();
        scriptCopie.chemin = positionCopie;
    }
}

