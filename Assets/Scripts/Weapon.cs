using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    private Animator anim;
    public GameObject player;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Attack();
        }
        faceMouse();
    }

    void faceMouse()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - player.transform.position.x, mousePosition.y - player.transform.position.y);

        transform.up = direction;

    }

}
