using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject player;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            playerAttack();
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

    void playerAttack()
    {
        anim.SetTrigger("PlayerAttack");
    }

}
