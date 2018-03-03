using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Actions/AttackBlob")]
public class AttackActionBlob : Action {


    public override void Act(StateController controller)
    {
        Debug.Log("ta");
        controller.enemyManager.TryAttack();
    }

}
