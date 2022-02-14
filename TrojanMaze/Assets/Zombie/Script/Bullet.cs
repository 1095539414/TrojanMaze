using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRigidbody;
    [SerializeField] float bulletSpeed = 10f;
    float bulletDamage = 0.05f;
    bool speedSet = false;
    private Vector2 _direction;

    private Vector2 dirToPlayer;
    private GameObject _player;

    private Vector2 GunPosition;
    private float xSpeed = 0f, ySpeed = 0f;


    void Start()
    {
        GunPosition = transform.position;
        bulletRigidbody = GetComponent<Rigidbody2D>();
        _player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        dirToPlayer = (Vector2)_player.transform.position - GunPosition;
        dirToPlayer.Normalize();
        if(speedSet == false) {
            ySpeed = dirToPlayer.y*bulletSpeed;
            xSpeed = dirToPlayer.x*bulletSpeed;
            Debug.Log("" + dirToPlayer.x + "  " + dirToPlayer.y);
            speedSet = true;
        }
        bulletRigidbody.velocity = new Vector2(xSpeed, ySpeed);
    }

    void OnTriggerEnter2D(Collider2D other) {

    }

    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag != "Zombie") {
            Destroy(gameObject);
        }
        if(other.collider.CompareTag("Player")) {
            iDamageable damageableObj = other.collider.gameObject.GetComponent<iDamageable>();
            if(damageableObj != null) {
                damageableObj.ReduceHealth(bulletDamage);
            }
        }
    }
}
