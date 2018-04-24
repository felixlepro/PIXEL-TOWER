using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconScript : MonoBehaviour {
    public Image iconImage;
    float effectTime = 0;
    float timeElapsed;


    // Update is called once per frame
    void Update() {
        timeElapsed += Time.deltaTime;
        iconImage.fillAmount = 1 - (timeElapsed / effectTime);
        if (timeElapsed >= effectTime)
        {
            timeElapsed = 0;
            effectTime = 0;
            this.gameObject.SetActive(false);
        }
	}

    public void IconSetup(float time)
    {
        if(time > effectTime) effectTime = time;
    }
}
