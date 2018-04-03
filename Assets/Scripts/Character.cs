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
    [HideInInspector] public bool CoroutineFire = false;
    [HideInInspector] public bool CoroutineIce;
    [HideInInspector] public const int maxStack = 5;
    [HideInInspector] public int currentStack;
    [HideInInspector] public float currentBurnTime;

    public abstract void RecevoirDegats(int damage, Vector3 kbDirection, float kbAmmount, float immuneTime);

    public void Burn(float burnTimer, int burnDamage)
    {
        currentBurnTime = 0;
        VerifStack();
        if (CoroutineFire == false)
        {
            StartCoroutine(IsBurning(burnTimer, burnDamage));
        }

    }

    public void VerifStack()
    {
        if (currentStack < maxStack)
        {
            currentStack += 1;
        }
    }
    IEnumerator IsBurning(float burnTime, int burnAmount)
    {

        CoroutineFire = true;
        while (currentBurnTime < burnTime)
        {
            currentBurnTime += Time.deltaTime;
            VerifStack();
            RecevoirDegats(burnAmount + currentStack, Vector3.zero, 0,0);
            yield return new WaitForSeconds(1f);
        }
        CoroutineFire = false;
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
