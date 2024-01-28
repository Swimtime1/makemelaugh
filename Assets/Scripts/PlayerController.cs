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

    private Animator animator;

    private SpriteRenderer sprite;

    float health = 100;

    const float CARTWHEEL_COOLDOWN_TIMER = 3;
    float cartwheelCooldown;
    bool performingCartwheel;
    Vector2 cartwheelSpeed;

    public AudioSource movement, soundFX;
    public AudioClip step, cartwheelSFX, ballonSwing;

    public GameObject grenadePrefab;

    Transform weapons, meleeWeapon, rangeWeapon;
    
    void Start() {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
        weapons = transform.GetChild(0);
        meleeWeapon = weapons.GetChild(0);
        rangeWeapon = weapons.GetChild(1);
    }

    void Update() {
        if(collider == null) { collider = GetComponent<Collider2D>(); }
        if(animator == null) { animator = GetComponent<Animator>(); }
        if(sprite == null) { sprite = GetComponent<SpriteRenderer>(); }

        if(weapons == null) { weapons = transform.GetChild(0); }
        if(meleeWeapon == null) { meleeWeapon = weapons.GetChild(0); }
        if(rangeWeapon == null) { rangeWeapon = weapons.GetChild(1); }


        //Cartwheel
        performingCartwheel = cartwheelCooldown > CARTWHEEL_COOLDOWN_TIMER - 0.5f;
        bool cartwheelInvinciblityActive = cartwheelCooldown > CARTWHEEL_COOLDOWN_TIMER - 0.7f;

        cartwheelCooldown -= Time.deltaTime;

        if(performingCartwheel) {
            transform.rotation *= Quaternion.Euler(0, 0, Time.deltaTime * 720 * (sprite.flipX ? 1 : -1));
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

        //Weapons
        if(playerMovement.magnitude > 0) {
            meleeWeapon.localScale = new Vector3(sprite.flipX ? -1 : 1, 1, 1);
            rangeWeapon.localPosition = playerMovement.normalized * 0.75f;
            rangeWeapon.localRotation = Quaternion.LookRotation(Vector3.forward, playerMovement.normalized) * Quaternion.Euler(0, 0, 90);
            if(rangeWeapon.localPosition.x != 0) {
                rangeWeapon.GetChild(0).GetComponent<SpriteRenderer>().flipY = rangeWeapon.localPosition.x < 0;
            }            
        }

        if(!performingCartwheel && !isDead) {
            if(!weapons.gameObject.activeSelf) { weapons.gameObject.SetActive(true); }
        } else {
            if(weapons.gameObject.activeSelf) { weapons.gameObject.SetActive(false); }
        }

        if(animator) {
            animator.SetBool("FaceScreen", movingUp);
            animator.SetBool("IsRunning", playerMovement.magnitude != 0f);
            animator.SetBool("IsCartwheeling", performingCartwheel);
            animator.SetBool("IsDead", isDead);
        }
    }

    void FixedUpdate() {
        if(rigidbody == null) { rigidbody = GetComponent<Rigidbody2D>(); }
    
        if(performingCartwheel) {
            rigidbody.velocity = cartwheelSpeed;
        } else {
            rigidbody.velocity = playerMovement;
        }

        // Plays foostep sound while the player is moving
        movement.mute = playerMovement.magnitude == 0;
    }

    public void MovementAction(InputAction.CallbackContext obj) {
        if(isDead) { return; }

        playerMovement = obj.ReadValue<Vector2>() * moveSpeed;

        if(playerMovement.magnitude > 0) {
            movingUp = playerMovement.y <= 0;
        }

        if(playerMovement.x != 0) {
            sprite.flipX = playerMovement.x < 0;
        }
    }

    public void Cartwheel(InputAction.CallbackContext obj) {
        if(isDead) { return; }

        if(cartwheelCooldown > 0) { return; }

        cartwheelSpeed = Vector2.zero;

        if(playerMovement.magnitude > 0) {
            cartwheelSpeed = playerMovement.normalized * 6;
        }

        cartwheelCooldown = CARTWHEEL_COOLDOWN_TIMER;

        collider.enabled = false;

        soundFX.PlayOneShot(cartwheelSFX);
    }

    public void AttackAction(InputAction.CallbackContext obj) {
        if(obj.canceled) {
            rangeWeapon.GetChild(0)?.GetComponent<Weapon>().StopAttack();
            meleeWeapon.GetChild(0)?.GetComponent<Weapon>().StopAttack();
        }
        
        if(!weapons.gameObject.activeSelf) { return; }

        if(obj.started) {
            if(rangeWeapon.gameObject.activeSelf) {
                rangeWeapon.GetChild(0)?.GetComponent<Weapon>().StartAttack();
            }

            if(meleeWeapon.gameObject.activeSelf) {
                meleeWeapon.GetChild(0)?.GetComponent<Weapon>().StartAttack();
            }
        }
    }

    public void SwitchWeapon(InputAction.CallbackContext obj) {
        if(!obj.started) { return; }

        rangeWeapon.GetChild(0)?.GetComponent<Weapon>().StopAttack();
        meleeWeapon.GetChild(0)?.GetComponent<Weapon>().StopAttack();

        rangeWeapon.gameObject.SetActive(!rangeWeapon.gameObject.activeSelf);
        meleeWeapon.gameObject.SetActive(!rangeWeapon.gameObject.activeSelf);

        Instantiate(grenadePrefab, transform.position, transform.rotation).GetComponent<Grenade>().ThrowGrenade(rangeWeapon.localPosition);
    }


    public void Hit(float damage) {
        health -= damage;
    }

    // Plays the Death Animations
    public void Death() {
        isDead = true;
        


        playerMovement = Vector2.zero;

        cartwheelSpeed = Vector2.zero;
        collider.enabled = true;
        cartwheelCooldown = 0;
    }

    // Plays the Revival Animation
    public void Revive() {
        isDead = false;
    }
}
