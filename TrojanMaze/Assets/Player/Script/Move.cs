using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour {
    [SerializeField] float speed = 10f;
    [SerializeField] private FieldOfView filedOfView;
    Vector2 moveInput;
    Rigidbody2D rigidBody;

    bool isBouncing = false;

    const float MAX_HP = 1f;
    static float HP;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        HP = MAX_HP;
    }

    void Update() {
        Run();
        FlipPlayer();
        filedOfView.SetOrigin(transform.position);

        if (HP > 1) {
            HP = 1;
        }
        // mute it because if it is kept,when hp is reduced to 0 by a trap, cannot reload the scene and send the player to the start position
        /*if (HP <= 0) {
            Destroy(gameObject);
        }*/

    }

    void Run() {
        Vector2 moveSpeed = new Vector2(moveInput.x * speed, moveInput.y * speed);
        rigidBody.velocity = moveSpeed;
    }

    void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    void FlipPlayer() {
        bool hasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
        if(hasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Zombie"))
        {
            DecreaseHP(0.1f);
        } 
    }

    public static void IncreaseHP(float value) {
        if(HP < MAX_HP) {
            HP += value;
        }
        if(HP > MAX_HP) {
            HP = MAX_HP;
        }
    }

    public static void DecreaseHP(float value) {
        if(HP > 0) {
            HP -= value;
        }
    }

    public static float GetHP() {
        return HP;
    }

}
