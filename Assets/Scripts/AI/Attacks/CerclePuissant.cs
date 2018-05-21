using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerclePuissant : MonoBehaviour {

    public const float chargeTime = 0.5f;
    Attacks attackStats;
    Collider2D attackHitbox;

    void Start()
    {
      
        
    }
    public void Setup(Attacks at)
    {
        attackStats = at;

        attackHitbox = GetComponent<Collider2D>();
        StartCoroutine(attack());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            attackHitbox.enabled = false;
            Player player = other.gameObject.GetComponent<Player>();
            player.SeFaitAttaquer(attackStats, Vector3.zero);
        }

    }

    IEnumerator attack()
    {
        yield return new WaitForSeconds(chargeTime);
        attackHitbox.enabled = true;
        yield return new WaitForSeconds(0.1f); ;
        attackHitbox.enabled = false;
        yield return new WaitForSeconds(0.4f);
        Destroy(this.gameObject);
    }
}
