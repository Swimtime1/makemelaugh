using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float bulletDamage;
    public float bulletSpeed;

    public float rotationSpeed;

    void Awake() {
        GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.up;
        GetComponent<Rigidbody2D>().AddTorque(rotationSpeed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Initialize(float damage, float speed) {
        bulletDamage = damage;
        bulletSpeed = speed;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if(collision.transform.gameObject.GetComponent<Projectile>()) {
            //Make explosion effect
            //Destroy both projectiles
            Destroy(collision.transform.gameObject);
            Destroy(gameObject);
        }
    }
}
