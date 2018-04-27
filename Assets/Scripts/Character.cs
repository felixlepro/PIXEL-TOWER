using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour {
    public int hp;
    public string nameC;
    public int maxHp;
   
    public Color wColor = Color.white;
    public float maxMoveSpeed;
    const float burnRepetitionTime = 1;
    private List<GameObject> iconList = new List<GameObject>();

    [HideInInspector] public bool immune = false;
    [HideInInspector] public bool stunned = false;
    [HideInInspector] public float currentSpeed;
 //   [HideInInspector] public bool CoroutineFire = false;
 //   [HideInInspector] public bool CoroutineIce;

    public abstract void RecevoirDegats(int damage, Vector3 kbDirection, float kbAmmount, float immuneTime);
    public abstract Vector3 PositionIcone();
    public GameObject canvas;

    public GameObject iconFeu;
    public GameObject iconGlace;

    public void Burn(float burnChance, int burnDamage,float burnTimer)
    {
        if (burnChance == 0)
        {
            return;
        }
        if (burnChance >= 100)
        {
            StartCoroutine(IsBurning(burnTimer, burnDamage));
        }
        else if (Random.value * 100 <= burnChance)
        {
            StartCoroutine(IsBurning(burnTimer, burnDamage));
        }           
    }

    public void SetUpIcon(int index, GameObject icon)
    {
        if(index == 1)
        {
            iconFeu = icon;
            iconFeu.SetActive(false);
        }
        else
        {
            iconGlace = icon;
            iconGlace.SetActive(false);
        }
    }

    IEnumerator IsBurning(float burnTime, int burnAmount)
    {
        iconFeu.SetActive(true);
        iconFeu.GetComponent<IconScript>().IconSetup(burnTime);
        float currentBurnTime = 0;
        while (currentBurnTime <= burnTime)
        {
            currentBurnTime += burnRepetitionTime;
            //VerifStack();
            RecevoirDegats(burnAmount, Vector3.zero, 0,0);
            yield return new WaitForSeconds(burnRepetitionTime);
        }
    }
    public void Slow(float slowChance,float slowAmount, float duration, bool fade)
    {
        
        if (slowChance <= 0)
        {
            return;
        }
        if (slowChance >= 100)
        {
            if (fade)
            {
                StartCoroutine(SlowFade(slowAmount, duration));
            }
            else
            {
                StartCoroutine(SlowNonFade(slowAmount, duration));
            }
        }
        else if (Random.value * 100 <= slowChance)
        {
            if (fade)
            {
                StartCoroutine(SlowFade(slowAmount, duration));
            }
            else
            {
                StartCoroutine(SlowNonFade(slowAmount, duration));
            }
        }

    }
    IEnumerator SlowNonFade(float slowAmount, float duration)
    {
        iconGlace.SetActive(true);
        iconGlace.GetComponent<IconScript>().IconSetup(duration);
        float time = 0;

        currentSpeed *= (1 - slowAmount);
        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        currentSpeed /= (1 - slowAmount);
    }
    IEnumerator SlowFade(float slowAmount, float duration)
    {
        iconGlace.SetActive(true);
        iconGlace.GetComponent<IconScript>().IconSetup(duration);
        float speed = 1f;
        float time = 0;
        while (time < duration)
        {
            currentSpeed /= speed;
            speed = (time / duration) * slowAmount + 1 - slowAmount;
            currentSpeed *= speed;

            time += Time.deltaTime;
            yield return null;
        }
        currentSpeed /= speed;
    }
    public void Freeze (float chance, float time)
    {
        if (chance == 0)
        {
            return;
        }
        if (chance >= 100)
        {
            Stun(time);
        }
       else if (Random.value * 100 <= chance)
        {
            Stun(time);
        }
    }
    public void Stun(float time)
    {
        stunned = true;
        Invoke("UnStun", time);
    }
    void UnStun()
    {
        stunned = false;
    }
}
