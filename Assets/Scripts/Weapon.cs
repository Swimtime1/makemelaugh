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

    // Start is called before the first frame update
    void Start() {
        if(projectilePrefab != null) {
            projectilePrefab.GetComponent<Projectile>().Initialize(damage, projectileSpeed);
        }
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
        cooldown = useSpeed;

        if(isRange && projectilePrefab != null) {
            for(int i = -spread / 2; i <= spread / 2; i++) {
                print(i);
                GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, i*10));
            }

        } else if(!isRange) {

        }
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
}
