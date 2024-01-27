using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float bulletDamage;
    public float bulletSpeed;

    void Awake() {
        GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.up;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Initialize(float damage, float speed) {
        bulletDamage = damage;
        bulletSpeed = speed;
    }
}
