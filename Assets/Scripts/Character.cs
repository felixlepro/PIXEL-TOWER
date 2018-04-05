using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public string name;
    public int maxHp;
    public int hp;
    public Color wColor = Color.white;
    public float maxMoveSpeed;

    [HideInInspector] public bool immune = false;
    [HideInInspector] protected bool stunned = false;
    [HideInInspector] public float currentSpeed;
 //   [HideInInspector] public bool CoroutineFire = false;
 //   [HideInInspector] public bool CoroutineIce;

    public abstract void RecevoirDegats(int damage, Vector3 kbDirection, float kbAmmount, float immuneTime);

    public void Burn(float burnTimer, int burnDamage)
    {
            StartCoroutine(IsBurning(burnTimer, burnDamage));
    }

    IEnumerator IsBurning(float burnTime, int burnAmount)
    {
        float currentBurnTime = 0;
        while (currentBurnTime < burnTime)
        {
            currentBurnTime += Time.deltaTime;
            //VerifStack();
            RecevoirDegats(burnAmount, Vector3.zero, 0,0);
            yield return new WaitForSeconds(1f);
        }
    }
    public void Slow(float slowAmount, float duration, bool fade)
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
    IEnumerator SlowNonFade(float slowAmount, float duration)
    {
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
