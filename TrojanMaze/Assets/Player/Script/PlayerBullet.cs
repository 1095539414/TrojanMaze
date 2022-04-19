using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NEW

public class PlayerBullet : MonoBehaviour {
    Rigidbody2D bulletRigidbody;
    [SerializeField] float bulletSpeed = 500f;
    float bulletDamage = 0.5f;
    private bool isExploding = false;

    private Vector2 _direction;

    public Animator animator;

    void Start() {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        _direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _direction.Normalize();
    }

    void FixedUpdate() {
        if (isExploding)
        {
            bulletRigidbody.velocity = Vector3.zero;
        }
        else
        {
            bulletRigidbody.velocity = _direction * bulletSpeed * Time.deltaTime;
        }
    }

    public IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Zombie") || other.CompareTag("Walls")) {
            isExploding = true;
            animator.SetBool("isExplosion", true);
            StartCoroutine(DestroyBullet());
            iDamageable damageableObj = other.gameObject.GetComponent<iDamageable>();
            if(damageableObj != null) {
                if(damageableObj.ReduceHealth(bulletDamage, this.gameObject)) {
                    Move.dmgByGun += bulletDamage;
                }
            }
        }
    }

}