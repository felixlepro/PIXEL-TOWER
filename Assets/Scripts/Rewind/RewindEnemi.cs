using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindEnemi : MonoBehaviour
{
    bool isRewinding = false;
    float tempsRewind;
    List<Vector2> positionRewind;
    public GameObject playerCopie;
    void Start()
    {
        positionRewind = new List<Vector2 >();
        tempsRewind = GameManager.instance.player.GetComponent<TimeRewinding>().nbSec;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && GameManager.instance.player.GetComponent<Player>().rewindCharge > 0)
            StartRewind();
        if (Input.GetKeyUp(KeyCode.R))
            StopRewind();
    }

    private void FixedUpdate()
    {
        if (isRewinding )
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
            transform.position = positionRewind[0];
            positionRewind.RemoveAt(0);
            transform.position = positionRewind[0];
            positionRewind.RemoveAt(0);
        }
        else
        {
            transform.position = positionRewind[0];
        }

    }
    void Record()
    {
        if (positionRewind.Count > Mathf.Round(tempsRewind  / Time.fixedDeltaTime))
            positionRewind.RemoveAt(positionRewind.Count - 1);
        
        positionRewind.Insert(0,transform.position);
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
       // GetComponentInParent<EnemyManager>().chaseTarget = playerCopie;
    }
}


