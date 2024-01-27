using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    private Vector2 playerMovement;
    private float moveSpeed = 3;

    private Rigidbody2D rb;

    public bool movingUp;

    public Animator animator;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        animator.SetBool("FaceScreen", movingUp);
        animator.SetBool("IsRunning", ((playerMovement.x != 0f) || (playerMovement.y != 0f)));
    }

    void FixedUpdate() {
        if(rb == null) { rb = GetComponent<Rigidbody2D>(); }

        rb.velocity = playerMovement;
    }

    public void MovementAction(InputAction.CallbackContext obj) {
        playerMovement = obj.ReadValue<Vector2>() * moveSpeed;

        print(playerMovement);

        movingUp = playerMovement.x < playerMovement.y;
    }

}
