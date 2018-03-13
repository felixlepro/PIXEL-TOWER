using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManager : WeaponManager {

    public GameObject bolt;
    private List<Bolt> boltList = new List<Bolt>();

    protected override void ChargeWeapon()
    {
        anim.SetBool("AttackCharge", true);
        chargeDoneRatio = (currentChargeTime / weapon.chargeTime);
       // anim.speed = anim.GetCurrentAnimatorStateInfo(0).length / weapon.chargeTime;
    }

    protected override void MaxChargeWeapon()
    {
        anim.speed = 1;
    }

    protected override void ReleaseChargedWeapon()
    {
        anim.speed = 1;
        anim.SetBool("AttackCharge", false);
        anim.SetTrigger("PlayerAttack");
        boltList.Add(Instantiate(bolt, transform.position, Quaternion.identity).GetComponent<Bolt>());

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0f);

        boltList[boltList.Count - 1].Setup(weapon.attackDamage, direction, weapon.knockBackAmount, 25 * chargeDoneRatio);
        ResetAttackTimer();
    }

    protected override void WeaponOnCD()
    {
    }
}

