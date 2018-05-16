using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinExplosion : MonoBehaviour {

    public float  forceMin = 125;
    public float forceScale = 150;
    float vitesse;
    Vector3 direction;
    int angle;
    AudioSource audio;
    Rigidbody2D rb;


	void Start () {
        //vitesse = Random.Range(vitesseMin, vitesseMax);
        angle = Random.Range(1, 360);
        direction =  new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)).normalized ;
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction.normalized * (Random.value*forceScale + forceMin));
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "MurPrototypeFond(Clone)" || other.name == "MurAvant(Clone)")
        {
            direction.y = -direction.y;
        }
        else if (other.name == "MurGauche(Clone)" || other.name == "MurDroit(Clone)")
        {
            direction.x = -direction.x;
        }
       // audio.Play();
    }
}
