using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Class : MonoBehaviour
{
    private Rigidbody2D rb;
    private MovementController2D movementController;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isDead = false;
    public float health = 100;
    private GameObject player, player2;
    private Vector2 followVector;
    private enum enemy_type {MeatShield, Bruiser, Rusher, Pistol, Shotgun, Machinegun};
    
    [SerializeField] Weapon weap_and_reap;

    [SerializeField] float minRange = 30;
    [SerializeField] float maxRange = 30;
    [SerializeField] float minShootrange = 30;
    [SerializeField] float maxShootrange = 30;

    [SerializeField] GameObject laughEffect;

    float laughTimer;

    // Update is called once per frame

    public bool IsAlive() {
        return !isDead;
    }

    public void Hit(float damage){
        if(isDead) { Destroy(this.gameObject); }

        health -= damage;

        if(laughTimer <= 0) {
            for(int i = 0; i < Mathf.Clamp(damage / 5, 1, 5); i++) {
                GameObject laugh = Instantiate(laughEffect, transform.position, Quaternion.Euler(-90, 0, 0));
                laugh.transform.localScale = Vector3.one * Mathf.Clamp(damage / 20, 0.35f, 0.75f);
            }
            laughTimer = 0.15f;
        }

        if(health <= 0) {
            isDead = true;
            animator.SetTrigger("Death");
            rb.velocity = Vector2.zero;
            if(weap_and_reap) {
                weap_and_reap.gameObject.SetActive(false);
            }
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    void Update() {
        laughTimer -= Time.deltaTime;
        if(laughTimer < 0) {
            laughTimer = 0;
        }
    }

    // FixedUpdate works off of physics update not on actual frames, won't slow the game down it just skips frames
    void FixedUpdate()
    {
        if(isDead) { return; }

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
            
            if(weap_and_reap) {
        
                weap_and_reap.StartAttack();

                if(weap_and_reap.gameObject == gameObject) { return; }
                
                Vector2 playerDir = cloest_playereerer.transform.position - transform.position;

                weap_and_reap.transform.parent.localPosition = playerDir.normalized * 0.75f;
                weap_and_reap.transform.parent.localRotation = Quaternion.LookRotation(Vector3.forward, playerDir.normalized) * Quaternion.Euler(0, 0, 90);
            }

        } else {
            if(weap_and_reap) {
                weap_and_reap.StopAttack();
            }
        }
    }
}
