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
    
    [SerializeField] Weapon weap_and_reap;

    [SerializeField] float minRange = 30;
    [SerializeField] float maxRange = 30;
    [SerializeField] float minShootrange = 30;
    [SerializeField] float maxShootrange = 30;

    [SerializeField] float movespeed = 3;

    private MovementController2D movementController;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
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
        if (movementController == null) { movementController = GetComponent<MovementController2D>(); }
        if (animator == null) { animator = GetComponent<Animator>(); }
        if (rb == null) { rb = GetComponent<Rigidbody2D>(); }
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }

        if(player == null) { 
            player = GameObject.Find("P1");
            player2 = GameObject.Find("P2");
        }

        if(player == null) {
            return;
        }

        GameObject cloest_playereerer = player2;

        if (player2 == null || Vector2.Distance(transform.position, player.transform.position) < Vector2.Distance(transform.position, player2.transform.position)     ){
            cloest_playereerer = player;
        }
        
        
        if (maxRange < (Vector2.Distance(transform.position, cloest_playereerer.transform.position))) {

            // GET CLOSER

            movementController.GetMoveCommand(player.transform.position);

            //followVector = (player.transform.position - transform.position).normalized * movespeed;
            //debug.Log("Text: " + Vector2.Distance(transform.position, player.transform.position));
            //rb.velocity = followVector;
        } else if ((Vector2.Distance(transform.position, cloest_playereerer.transform.position) < minRange)) {

            // GET FARTHER
            movementController.GetMoveCommand( transform.position - player.transform.position);


        } else if ( Mathf.Abs(Vector2.Distance(transform.position, cloest_playereerer.transform.position) - ((minRange + maxRange) * 0.5f) ) < 0.1f) {
            rb.velocity = Vector2.zero;
        }

        spriteRenderer.flipX = rb.velocity.x > 0;
        animator.SetBool("IsWalking", rb.velocity.sqrMagnitude > 0);


        if (maxShootrange > (Vector2.Distance(transform.position, cloest_playereerer.transform.position)) &&
            (Vector2.Distance(transform.position, cloest_playereerer.transform.position) > minShootrange)) {
            
            weap_and_reap.StartAttack();

            Vector2 playerDir = cloest_playereerer.transform.position - transform.position;

            weap_and_reap.transform.parent.localPosition = playerDir.normalized * 0.75f;
            weap_and_reap.transform.parent.localRotation = Quaternion.LookRotation(Vector3.forward, playerDir.normalized) * Quaternion.Euler(0, 0, 90);

        } else {
            weap_and_reap.StopAttack();
        }
    }
}
