using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconScript : MonoBehaviour {
    public Image iconImage;
    float effectTime;
    float timeElapsed;


	// Update is called once per frame
	void Update () {
        timeElapsed += Time.deltaTime;
        iconImage.fillAmount = 1 - (timeElapsed / effectTime);
        if (timeElapsed >= effectTime) Destroy(this.gameObject);
	}

    public void IconSetup(float time)
    {
        effectTime = time;
    }
}
