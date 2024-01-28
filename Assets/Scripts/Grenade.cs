using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    float grenadeTimer;

    const float AIR_TIME = 0.75f;
    const float SMOKE_TIME = 1.5f;
    const float EXPLODE_TIME = 10;
    const float FADE_TIME = 10;

    bool landed = false;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        grenadeTimer = 0;
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ThrowGrenade(Vector2 speed) {
        if(rigidbody == null) { rigidbody = GetComponent<Rigidbody2D>(); }

        rigidbody.velocity = speed * 10;

        rigidbody.AddTorque(1f, ForceMode2D.Impulse);

        transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update() {
        if(rigidbody == null) { rigidbody = GetComponent<Rigidbody2D>(); }
        if(spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }

        grenadeTimer += Time.deltaTime;

        float opacity = Mathf.Clamp((SMOKE_TIME + EXPLODE_TIME + FADE_TIME - grenadeTimer)/FADE_TIME, 0, 1);

        spriteRenderer.color = new Color(1, 1, 1, opacity);

        if(opacity <= 0) { Destroy(gameObject); }

        if(transform.childCount == 0) { return; }

        transform.GetChild(0).rotation = Quaternion.Euler(-90, 0, 0);

        if(grenadeTimer >= AIR_TIME && !landed) {
            landed = true;
            rigidbody.angularDrag = 1;
            rigidbody.drag = 1;
        }

        if(grenadeTimer > SMOKE_TIME && (bool) !transform.GetChild(0)?.gameObject.activeSelf) {
            transform.GetChild(0)?.gameObject.SetActive(true);  
        }
    }
}
