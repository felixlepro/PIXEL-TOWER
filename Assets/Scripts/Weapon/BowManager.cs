using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowManager : WeaponManager {
    public float boltSpeed;
    public float slowAmount;

    public GameObject bolt;
    private List<Bolt> boltList = new List<Bolt>();

    protected override void ChargeWeapon()
    {
       
        anim.SetBool("AttackCharge", true);
        player.currentSpeed /= (1 - (slowAmount * (1 - (1 - chargeDoneRatio) * (1 - chargeDoneRatio))));
        chargeDoneRatio = (currentChargeTime / chargeTime);
        player.currentSpeed *= (1 - (slowAmount * (1-(1-chargeDoneRatio)*(1-chargeDoneRatio))));

    }

    protected override void MaxChargeWeapon()
    {
    }

    protected override void ReleaseChargedWeapon()
    {
        player.currentSpeed /= (1 - (slowAmount * (1 - (1 - chargeDoneRatio) * (1 - chargeDoneRatio))));

        anim.SetBool("AttackCharge", false);
        anim.SetTrigger("PlayerAttack");
        boltList.Add(Instantiate(bolt, transform.position, Quaternion.identity).GetComponent<Bolt>());

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0f);

        boltList[boltList.Count - 1].Setup(attackDamage, direction, knockBackAmount, boltSpeed * chargeDoneRatio, boltSpeed);
        ResetAttackTimer();
    }

    protected override void WeaponOnCD()
    {
    }
}

