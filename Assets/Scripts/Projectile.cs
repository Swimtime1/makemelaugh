using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosion;

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
        //Make explosion effect
        Instantiate(explosion, transform.position, transform.rotation);

        if(collision.transform.gameObject.GetComponent<Projectile>()) {
            Destroy(collision.transform.gameObject);
        }

        Destroy(gameObject);

    }
}
