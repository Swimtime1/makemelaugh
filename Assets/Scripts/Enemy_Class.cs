using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Class : MonoBehaviour
{
    private Rigidbody2D rb;
    private int health;
    //private enum range {melee = 1.0f, shotgun = 5.0f, pistol = 10.0f, stationary = 0.0f};
    private int damage;
    private GameObject player;
    private Vector2 followVector;


    public float minRange = 30;
    public float movespeed = 3;
    
    // Start is called before the first frame update
    void Start()
    {
       
        
    }   

    // Update is called once per frame

    // FixedUpdate works off of physics update not on actual frames, won't slow the game down it just skips frames
    void FixedUpdate()
    {
        if (rb == null){
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.Find("Square");
        }
        //rb.velocity = new Vector2 (2f, 2f);
        
        if (Vector2.Distance(transform.position, player.transform.position) > minRange){
            //followVector = (player.transform.position - transform.position).normalized * movespeed;
            //Debug.Log("Text: " + Vector2.Distance(transform.position, player.transform.position));

            //rb.velocity = followVector;
        }
        else {
            //rb.velocity = Vector2.zero;
        }

        //followVector = Vector2.MoveTowards(transform.position, player.transform.position, 0.06f);
        //rb.MovePosition(followVector);
    }
}
