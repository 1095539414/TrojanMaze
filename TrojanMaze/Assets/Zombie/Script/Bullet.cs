using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BuffItem {
    Rigidbody2D bulletRigidbody;
    [SerializeField] float bulletSpeed = 10f;
    float bulletDamage = 0.01f;
    bool speedSet = false;
    private Vector2 _direction;

    private Vector2 dirToPlayer;
    private GameObject _player;

    private Vector2 GunPosition;
    private float xSpeed = 0f, ySpeed = 0f;
    private float _speedDecreaseRatio = 2f;
    private float _speedDecreaseDuration = 2f;

    void Start() {
        GunPosition = transform.position;
        bulletRigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        dirToPlayer = (Vector2)_player.transform.position - GunPosition;
        dirToPlayer.Normalize();
        if(speedSet == false) {
            ySpeed = dirToPlayer.y * bulletSpeed;
            xSpeed = dirToPlayer.x * bulletSpeed;
            // Debug.Log("" + dirToPlayer.x + "  " + dirToPlayer.y);
            speedSet = true;
        }
        bulletRigidbody.velocity = new Vector2(xSpeed, ySpeed);
    }

    protected override bool AddBuff() {
        iDamageable damageableObj = buffTarget.gameObject.GetComponent<iDamageable>();
        if(damageableObj != null) {
            damageableObj.ReduceHealth(bulletDamage);
            Move._move.speed /= _speedDecreaseRatio;
            return true;
        }
        return false;
    }

    protected override bool RemoveBuff() {
        Move._move.speed *= _speedDecreaseRatio;
        return true;
    }

    protected override float GetDuration() {
        return _speedDecreaseDuration;
    }
}
