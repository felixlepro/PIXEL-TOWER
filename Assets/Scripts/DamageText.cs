using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    RectTransform rt;
    public Animator anim;
    private Text damageText;
    Vector3 location;

    void OnEnable()
    {
        AnimatorClipInfo[] info = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, info[0].clip.length*0.95f);
        damageText = anim.GetComponent<Text>();
        rt = GetComponent<RectTransform>();
        location = Vector3.zero;
    }


    public void SetText(int text, Vector3 loc)
    {
        location = loc;
        float newSize = (float)text / 100 + 1; 
        rt.localScale *= newSize; 
        damageText.text = text.ToString();
    }

    private void Update()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.x , location.y));
        rt.position = screenPosition;
        rt.localScale -= rt.localScale*0.6f * Time.deltaTime;
    }
}
