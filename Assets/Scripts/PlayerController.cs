using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    private Vector2 playerMovement;
    private float moveSpeed = 3;

    private Rigidbody2D rigidbody;
    private Collider2D collider;
    public bool movingUp;
    public bool isDead;

    public Animator animator;

    float health = 100;

    const float CARTWHEEL_COOLDOWN_TIMER = 3;
    float cartwheelCooldown;
    bool performingCartwheel;
    Vector2 cartwheelSpeed;

    public AudioSource movement;
    public AudioClip step, ballonSwing;

    void Update() {
        if(collider == null) { collider = GetComponent<Collider2D>(); }

        performingCartwheel = cartwheelCooldown > CARTWHEEL_COOLDOWN_TIMER - 0.5f;
        bool cartwheelInvinciblityActive = cartwheelCooldown > CARTWHEEL_COOLDOWN_TIMER - 0.7f;

        cartwheelCooldown -= Time.deltaTime;

        if(performingCartwheel) {
            transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * 720 * -transform.localScale.x);
        } else {
            cartwheelSpeed = Vector2.zero;
            transform.rotation = Quaternion.identity;
        } 
        
        if (!cartwheelInvinciblityActive && !collider.enabled) {
            collider.enabled = true;
        }

        if(cartwheelCooldown < 0) {
            cartwheelCooldown = 0;
        }

        if(animator) {
            animator.SetBool("FaceScreen", movingUp);
            animator.SetBool("IsRunning", playerMovement.magnitude != 0f);
            animator.SetBool("IsCartwheeling", performingCartwheel);
        }
    }

    void FixedUpdate() {
        // Only allows movement if the player is alive
        if(!isDead){
            if(rigidbody == null) { rigidbody = GetComponent<Rigidbody2D>(); }
        
            if(performingCartwheel) {
                rigidbody.velocity = cartwheelSpeed;
            } else {
                rigidbody.velocity = playerMovement;
            }

            // Plays foostep sound while the player is moving
            if(playerMovement.magnitude != 0) {
                movement.mute = false;
            }
            else { movement.mute = true; }
        }
    }

    public void MovementAction(InputAction.CallbackContext obj) {
        playerMovement = obj.ReadValue<Vector2>() * moveSpeed;

        if(playerMovement.magnitude > 0) {
            movingUp = playerMovement.y <= 0;
        }

        if(playerMovement.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (playerMovement.x > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void Cartwheel(InputAction.CallbackContext obj) {
        if(collider == null) { collider = GetComponent<Collider2D>(); }

        if(cartwheelCooldown > 0) { return; }

        cartwheelSpeed = Vector2.zero;

        if(playerMovement.magnitude > 0) {
            cartwheelSpeed = playerMovement.normalized * 6;
        }

        cartwheelCooldown = CARTWHEEL_COOLDOWN_TIMER;

        collider.enabled = false;
    }

    public void Hit(float damage) {
        health -= damage;
    }

    // Plays the Death Animations
    public void Death() {
        animator.SetBool("IsDead", true);
        isDead = true;
    }

    // Plays the Revival Animation
    public void Revive() {
        animator.SetBool("IsDead", false);
        isDead = false;
    }
}
