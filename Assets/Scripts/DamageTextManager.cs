using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour {

    private static DamageText popupText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if (!popupText)
            popupText = Resources.Load<DamageText>("Prefabs/PopUpTextParent");
    }

    public static void CreateFloatingText(int text, Vector3 location)
    {
        DamageText instance = Instantiate(popupText);
        Vector3 offset = new Vector3(Random.Range(0.4f, 0.8f), Random.Range(0.5f, 1.2f), 0);
        location += offset;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.x, location.y));

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text,location);
    }
}
