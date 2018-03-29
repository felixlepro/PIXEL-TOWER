using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attacks {

    public float projectileSpeed;
    public override float speed    
    {
        get
        {
            return projectileSpeed;
        }
        set
        {
            projectileSpeed = value;
        }
    }

}
