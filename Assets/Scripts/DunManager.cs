using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunManager : MonoBehaviour {

    Vector3 position;

	void Update () {
        transform.position = position;
	}

    public void Initialize(Vector3 pos)
    {
        position = pos;
        Animator anim = GetComponent<Animator>();
        AnimatorClipInfo[] info = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, info[0].clip.length * 0.95f);
    }
}
