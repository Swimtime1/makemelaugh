using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Class : MonoBehaviour
{
    private Rigidbody2D rb;
    private int health = 100;
    private int damage = 10;
    private GameObject player, player2;
    private Vector2 followVector;
    private enum enemy_type {MeatShield, Bruiser, Rusher, Pistol, Shotgun, Machinegun};
    private Weapon weap_and_reap;

    [SerializeField] float minRange = 30;
    [SerializeField] float maxRange = 30;
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
        if (rb == null){ rb = GetComponent<Rigidbody2D>(); }
        if(player == null) { 
            player = GameObject.Find("P1");
            player2 = GameObject.Find("P2");
        }

        
        
        if (maxRange < (Vector2.Distance(transform.position, player.transform.position))) {

            // GET CLOSER




            followVector = (player.transform.position - transform.position).normalized * movespeed;
            //debug.Log("Text: " + Vector2.Distance(transform.position, player.transform.position));
            //rb.velocity = followVector;
        } else if ((Vector2.Distance(transform.position, player.transform.position) < minRange)) {

            // GET FARTHER

        } else {
            rb.velocity = Vector2.zero;
        }

        if (maxShootrange > (Vector2.Distance(transform.position, player.transform.position)) &&
            (Vector2.Distance(transform.position, player.transform.position) > minShootrange)) {
            Debug.Log("Shoot");
        }
        else {
            Debug.Log("Stop Shooting");
        }
    }
}
