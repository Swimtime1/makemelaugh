using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject explosion;
    public List<Sprite> sprites;

    SpriteRenderer spriteRenderer;

    float bulletDamage;
    public float bulletSpeed;

    public float rotationSpeed;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];

        GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.up;
        GetComponent<Rigidbody2D>().AddTorque(rotationSpeed, ForceMode2D.Impulse);
    }

    public void Initialize(float damage, float speed) {
        bulletDamage = damage;
        bulletSpeed = speed;
    }

    public void OnTriggerEnter2D(Collider2D collision) {            
        PlayerController player = collision.transform.gameObject.GetComponent<PlayerController>();
        Enemy_Class enemy = collision.transform.gameObject.GetComponent<Enemy_Class>();

        if(player) {
            player.Hit(bulletDamage);
        } else if (enemy) {
            enemy.Hit(bulletDamage);
        } else {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        Destroy(gameObject);

    }
}
