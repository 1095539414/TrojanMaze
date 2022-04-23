using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BuffItem {
    Rigidbody2D bulletRigidbody;
    [SerializeField] float bulletSpeed = 10f;
    float bulletDamage = 0.1f;
    private Vector2 dirToPlayer;
    private GameObject _player;

    private Vector2 GunPosition;
    private float _speedDecreaseRatio = 2f;
    private float _speedDecreaseDuration = 2f;

    void Start() {
        GunPosition = transform.position;
        bulletRigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.FindWithTag("Player");
        dirToPlayer = (Vector2)_player.transform.position - GunPosition;
        // transform.rotation = Quaternion.LookRotation(Vector3.forward, dirToPlayer);
        if(dirToPlayer.x < 0) {
            Vector3 scale = transform.localScale;
            scale.y *= -1;
            transform.localScale = scale;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        dirToPlayer.Normalize();
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        bulletRigidbody.velocity = new Vector2(dirToPlayer.x * bulletSpeed, dirToPlayer.y * bulletSpeed);
    }

    protected override bool AddBuff() {
        iDamageable damageableObj = buffTarget.gameObject.GetComponent<iDamageable>();
        if(damageableObj != null) {
            damageableObj.ReduceHealth(bulletDamage, this.gameObject);
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
