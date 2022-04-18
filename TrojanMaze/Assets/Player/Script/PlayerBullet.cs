using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NEW

public class PlayerBullet : MonoBehaviour {
    Rigidbody2D bulletRigidbody;
    [SerializeField] float bulletSpeed = 500f;
    float bulletDamage = 0.5f;

    private Vector2 _direction;

    void Start() {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        _direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _direction.Normalize();
    }

    void FixedUpdate() {
        bulletRigidbody.velocity = _direction * bulletSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Zombie") || other.CompareTag("Walls")) {
            Destroy(gameObject);
            iDamageable damageableObj = other.gameObject.GetComponent<iDamageable>();
            if(damageableObj != null) {
                if(damageableObj.ReduceHealth(bulletDamage, this.gameObject)) {
                    Move.dmgByGun += bulletDamage;
                }
            }
        }
    }

}