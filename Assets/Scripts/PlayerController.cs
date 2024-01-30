using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    private Vector2 playerMovement, weaponHeading;
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

    public GameObject clownBlood;

    private float hitTimer;

    private int numGrenades;
    
    void Start() {
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        
        weapons = transform.GetChild(0);
        meleeWeapon = weapons.GetChild(0);
        rangeWeapon = weapons.GetChild(1);

        numGrenades = 0;
    }

    void Update() {
        if(collider == null) { collider = GetComponent<Collider2D>(); }
        if(animator == null) { animator = GetComponent<Animator>(); }
        if(sprite == null) { sprite = GetComponent<SpriteRenderer>(); }

        if(weapons == null) { weapons = transform.GetChild(0); }
        if(meleeWeapon == null) { meleeWeapon = weapons.GetChild(0); }
        if(rangeWeapon == null) { rangeWeapon = weapons.GetChild(1); }


        hitTimer -= Time.deltaTime;
        if(hitTimer < 0) {
            hitTimer = 0;
        }

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

        if (!cartwheelInvinciblityActive && !isDead) {
            collider.excludeLayers = 1 << 7 | 1 << 8 | 1 << 9;
        } else {
            collider.excludeLayers = 1 << 7 | 1 << 8 | 1 << 9 | 1 << 10;
        }

        if(cartwheelCooldown < 0) {
            cartwheelCooldown = 0;
        }

        //Weapons
        if(playerMovement.magnitude > 0) {
            meleeWeapon.localScale = new Vector3(sprite.flipX ? -1 : 1, 1, 1);  

            weaponHeading = playerMovement.normalized;        
        }

        rangeWeapon.localRotation = Quaternion.LookRotation(Vector3.forward, weaponHeading.normalized) * Quaternion.Euler(0, 0, 90);
        rangeWeapon.localPosition = weaponHeading.normalized * 0.75f;
        if(rangeWeapon.localPosition.x != 0) {
            rangeWeapon.GetChild(0).GetComponent<SpriteRenderer>().flipY = rangeWeapon.localPosition.x < 0;
        }  

        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 15, 1 << 9);
        float bestDist = 15*15;
        for(int i = 0; i < enemies.Length; i++) {
            //Is this enemy within a 35 degrees of our current aim?
            if(Vector2.Angle(weaponHeading.normalized, (enemies[i].transform.position-transform.position).normalized) > 45) { continue; }
            
            //Is this enemy closer?
            if( (transform.position - enemies[i].transform.position).sqrMagnitude > bestDist) { continue; }

            //Could be a good target to aim at, make sure it's not blocked
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (enemies[i].transform.position - transform.position).normalized, 20, 1 << 9 | 1 << 11);
            //Hit an obstacle, not an enemy
            if(hit.collider != enemies[i]) { continue; }
            
            //Make sure the enemy is actually alive first
            if(!enemies[i].GetComponent<Enemy_Class>().IsAlive()) { continue; }

            //Current best enemy!
            bestDist = (transform.position - enemies[i].transform.position).sqrMagnitude;

            rangeWeapon.localRotation = Quaternion.LookRotation(Vector3.forward, (enemies[i].transform.position - transform.position).normalized) * Quaternion.Euler(0, 0, 90);
            rangeWeapon.localPosition = (enemies[i].transform.position - transform.position).normalized * 0.75f;
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
        }

        // Caps the number of grenades the Player can have at 5
        if(numGrenades > 5) { numGrenades = 5; }
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

        cartwheelSpeed = playerMovement.normalized * 6;

        cartwheelCooldown = CARTWHEEL_COOLDOWN_TIMER;

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

        // Instantiate(grenadePrefab, transform.position, transform.rotation).GetComponent<Grenade>().ThrowGrenade(rangeWeapon.localPosition);
    }


    public void Hit(float damage) {
        if(isDead) { return; }

        health -= damage;

        if(hitTimer <= 0) {
            for(int i = 0; i < Mathf.Clamp(damage / 5, 1, 5); i++) {
                GameObject blood = Instantiate(clownBlood, transform.position, Quaternion.Euler(-90, 0, 0));
                blood.transform.localScale = Vector3.one * Mathf.Clamp(damage / 3, 0.75f, 1.15f);
            }
            hitTimer = 0.15f;
        }

        if(health <= 0) {
            Death();
        }
    }

    // Plays the Death Animations
    public void Death() {
        isDead = true; 

        animator.SetTrigger("Death");

        playerMovement = Vector2.zero;

        cartwheelSpeed = Vector2.zero;
        cartwheelCooldown = 0;
    }

    // Plays the Revival Animation
    public void Revive() {
        isDead = false;
        animator.SetTrigger("Revival");
    }

    // Returns the Player's health
    public float GetHealth() {
        return this.health;
    }

    // Returns the number of grenades the Player has
    public int GetGrenades() {
        return this.numGrenades;
    }

    public bool IsAlive() {
        return !isDead;
    }
}
