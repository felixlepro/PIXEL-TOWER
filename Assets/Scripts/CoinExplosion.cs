using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinExplosion : MonoBehaviour {

    public float  vitesseMin;
    public float vitesseMax;
    float vitesse;
    Vector3 direction;
    int angle;
    AudioSource audio;
    Rigidbody2D rb;


	void Start () {
        vitesse = Random.Range(vitesseMin, vitesseMax);
        angle = Random.Range(1, 360);
        direction =  new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)).normalized ;
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
       

       // transform.position += direction * vitesse;
        if (vitesse > 0.0005)
        {
            vitesse -= vitesse*Time.deltaTime;
        }
        else vitesse = 0;

        rb.MovePosition(transform.position + direction.normalized * vitesse);
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
        audio.Play();
    }
    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.tag == "Obstacle")
    //    {
    //        Debug.Log("wtf");
    //    }
    //}
    //private void OnDestroy()
    //{
    //    audio.Play();
    //    //GameManager.instance..PlaySound();
    //}
}
