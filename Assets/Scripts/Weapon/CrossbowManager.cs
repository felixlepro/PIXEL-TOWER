using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowManager : WeaponManager
{
    public float boltSpeed;


    public GameObject bolt;
    private List<Bolt> boltList = new List<Bolt>();

    public override void WeaponSetStats()
    {
        throw new System.NotImplementedException();
    }

    protected override void ChargeWeapon()
    {
        
    }

    protected override void MaxChargeWeapon()
    {
        
    }

    protected override void ReleaseChargedWeapon()
    {
        anim.SetTrigger("isFireing");
        boltList.Add(Instantiate(bolt, transform.position, Quaternion.identity).GetComponent<Bolt>());

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0f);

        boltList[boltList.Count - 1].Setup(attackDamage, direction, knockBackAmount, boltSpeed, boltSpeed);
        ResetAttackTimer();
    }

    protected override void WeaponOnCD()
    {
        
    }
}
