using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Class : MonoBehaviour
{
    private Rigidbody2D rb;
    private int health = 100;
    private int damage = 10;
    private GameObject player;
    private Vector2 followVector;
    private enum enemy_type {MeatShield, Bruiser, Rusher, Pistol, Shotgun, Machinegun};
    private Weapon weap_and_reap = new Weapon();

    private bool Aiden_Crossfield_Mode = false;

    [SerializeField] float minRange = 30;
    [SerializeField] float minRange = 30;
    [SerializeField] float minShootrange = 30;
    [SerializeField] float maxShootrange = 30;


    [SerializeField] float movespeed = 3;
    
    // Start is called before the first frame update
    void Start()
    {
       
        
    }   

    // Update is called once per frame

    void hit(int Damage){
        health -= Damage;
    }


    // FixedUpdate works off of physics update not on actual frames, won't slow the game down it just skips frames
    void FixedUpdate()
    {
        if (rb == null){
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.Find("P1");
        }
        
        if (max_range < (Vector2.Distance(transform.position, player.transform.position))) {

            // GET CLOSER




            followVector = (player.transform.position - transform.position).normalized * movespeed;
            //debug.Log("Text: " + Vector2.Distance(transform.position, player.transform.position));
            //rb.velocity = followVector;
        }

        if ((Vector2.Distance(transform.position, player.transform.position) < minRange)) {

            // GET FARTHER

        }

        if (maxShootrange > (Vector2.Distance(transform.position, player.transform.position)) &&
            (Vector2.Distance(transform.position, player.transform.position) > minShootrange)) {

            rb.velocity = Vector2.zero;
            Debug.Log("Shoot");
        }
        else {
            cry
        }

        if (health <= 0){
            Debug.Log("Dead");
        }

    }
}
