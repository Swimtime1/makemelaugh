using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public bool isRange;
    public bool autoUse;
    public GameObject projectilePrefab;
    public float useSpeed;
    public float damage;
    public float projectileSpeed;

    public int spread = 1;

    private float cooldown = 0;
    public bool isAttacking = false;

    private Animator animator;

    private Collider2D collider;

    // Start is called before the first frame update
    void Start() {
        if(projectilePrefab != null) {
            projectilePrefab.GetComponent<Projectile>().Initialize(damage, projectileSpeed);
        }

        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    void Update() {
        if(animator == null) { animator = GetComponent<Animator>(); }
        if(collider == null) { collider = GetComponent<Collider2D>(); }
    }

    void FixedUpdate() {
        if(isAttacking && cooldown <= 0 && autoUse) {
            DoAttack();
        }

        cooldown -= Time.deltaTime;
        
        if(cooldown <= 0) {
            cooldown = 0;
        }
    }

    void DoAttack() {
        animator.SetTrigger("OneShotAttack");

        cooldown = useSpeed;
    }

    public void StartAttack() {
        if(!autoUse && cooldown <= 0) {
            DoAttack();
        } else {
            isAttacking = true; 
        }
    }

    public void StopAttack() {
        isAttacking = false;
    }

    public void OnTriggerEnter2D(Collider2D collision) { 
        if(isRange) { return; }

        //If melee, deal damage
    }

    public void EnableHurtbox() {
        collider.enabled = true;
    }

    public void DisableHurtbox() {
        collider.enabled = false;
    }

    public void Shoot() {
        if(isRange && projectilePrefab != null) {
            for(int i = -spread / 2; i <= spread / 2; i++) {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, -90) * Quaternion.Euler(0, 0, i*10));
            }
        }
    }
}
