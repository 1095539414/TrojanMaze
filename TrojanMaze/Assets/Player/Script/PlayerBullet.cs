using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NEW

public class PlayerBullet : MonoBehaviour {
    Rigidbody2D bulletRigidbody;
    [SerializeField] float bulletSpeed = 10f;
    float bulletDamage = 0.5f;
    Move player;
    bool speedSet = false;
    private Vector2 _direction;
    private float _time;
    private float last_x;
    private float last_y;
    private Vector2 dirToPlayer;


    void Start() {
        bulletRigidbody = GetComponent<Rigidbody2D>();

        player = FindObjectOfType<Move>();
        _direction = player.transform.right;
        _time = 0;
        Collider c = player.GetComponent<Collider>();
        //Physics.IgnoreCollision(c,gameObject.GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update() {
        _time += Time.deltaTime;
        // RaycastHit2D hit = Physics2D.CircleCast((Vector2)player.transform.position + _direction, 5f, _direction);
        // if(hit && hit.collider.CompareTag("Zombie")) {
        //     _direction = hit.collider.gameObject.transform.position - transform.position;
        //     _direction.Normalize();
        if(speedSet == false) {
            Rigidbody2D playSpeed = player.GetComponent<Rigidbody2D>();
            Vector2 playerSpeed = playSpeed.velocity;
            if(playerSpeed.x != 0 && Math.Abs(playerSpeed.x) > 1) last_x = playerSpeed.x;
            if(playerSpeed.y != 0 && Math.Abs(playerSpeed.y) > 1) last_y = playerSpeed.y;
            float ySpeed = 0;
            float xSpeed = 0;
            if(Math.Abs(last_x) >= Math.Abs(last_y)) {
                ySpeed = 0;
                xSpeed = last_x >= 0 ? bulletSpeed : -1 * bulletSpeed;

            } else {
                ySpeed = last_y >= 0 ? bulletSpeed : -1 * bulletSpeed;
                xSpeed = 0;
            }

            bulletRigidbody.velocity = new Vector2(xSpeed, ySpeed);
            speedSet = true;
        }
        // } 
        //
        // if(speedSet == false & _time > 10 * Time.deltaTime) {
        //     bulletRigidbody.velocity = new Vector2(Random.Range(0, 10) > 5 ? bulletSpeed : -bulletSpeed, 0);
        //     speedSet = true;
        // }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Zombie") || other.CompareTag("Walls")) {
            Destroy(gameObject);
            iDamageable damageableObj = other.gameObject.GetComponent<iDamageable>();
            if(damageableObj != null) {
                damageableObj.ReduceHealth(bulletDamage);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {

    }
}