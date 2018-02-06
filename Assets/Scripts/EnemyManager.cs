using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public Enemy enemy;
    private SpriteRenderer spriteR;
    private Animator anim;

    float Distance;
    public Transform Target;
    float lookAtDistance;
     float chaseRange;
     float attackRange;
     float moveSpeed;
    float Damping = 6f;
    float attackRepeatTime;
     int TheDammage;

     private float attackTime;

    CharacterController  controller;
    private Vector3 moveDirection;
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.color = enemy.wColor;
        anim = GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = enemy.animator;;

        lookAtDistance = enemy.lookAtDistance;
         chaseRange = enemy.chaseRange;
         attackRange = enemy.attackRange;
         moveSpeed = enemy.moveSpeed;

         attackRepeatTime = enemy.attackSpeed;
         TheDammage = enemy.attackDamage;
        controller = GetComponent<CharacterController>();
       

        attackTime = Time.time;
    }

    void Update()
    {
       // if (RespawnMenuV2.playerIsDead == false)
        {
            Distance = Vector3.Distance(Target.position, transform.position);

            if (Distance < lookAtDistance)
            {
                //lookAt();
            }

            if (Distance > lookAtDistance)
            {
                spriteR.material.color = Color.green;
            }

            if (Distance < attackRange)
            {
                attack();
            }
            else if (Distance < chaseRange)
            {
                chase();
            }
        }
    }

    void lookAt()
    {
        spriteR.material.color = Color.yellow;
        var rotation = Quaternion.LookRotation(Target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);
    }

    void chase()
    {
        spriteR.material.color = Color.red;

        moveDirection = transform.forward;
        moveDirection *= moveSpeed;

//        moveDirection.y -= gravity * Time.deltaTime;
        //controller.Move(moveDirection * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, Target.position,Time.deltaTime * moveSpeed/Distance);
        Debug.Log("caca");
    }

    void attack()
    {
        if (Time.time > attackTime)
        {
            //Target.SendMessage("ApplyDammage", TheDammage);
            //Debug.Log("The Enemy Has Attacked");
            attackTime = Time.time + attackRepeatTime;
        }
    }

    void ApplyDammage()
    {
        chaseRange += 30;
        moveSpeed += 2;
        lookAtDistance += 40;
    }

}
