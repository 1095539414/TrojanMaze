using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bulletRigidbody;
    [SerializeField] float bulletSpeed = 8f;
    SimpleZombie zombie;
    bool speedSet = false;
    private Vector2 _direction;
    private float _time;


    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        zombie = FindObjectOfType<SimpleZombie>();
        _direction = zombie.transform.right;
        _time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast((Vector2)zombie.transform.position + _direction, 5f, _direction);
        if(hit && hit.collider.CompareTag("Player")) {
            _direction = hit.collider.gameObject.transform.position - transform.position;
            _direction.Normalize();
            if(speedSet == false) {
                float ySpeed = _direction.y*bulletSpeed;
                float xSpeed = _direction.x == 0 ? bulletSpeed : _direction.x*bulletSpeed;
                bulletRigidbody.velocity = new Vector2(xSpeed, ySpeed);
                speedSet = true;
            }
        } 

        if(speedSet == false & _time > 10 * Time.deltaTime) {
            bulletRigidbody.velocity = new Vector2(bulletSpeed, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("OnTriggerEnter2D");
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("OnCollisionEnter2D");
        if(other.gameObject.tag != "Zombie") {
            Destroy(gameObject);
        }
    }
}
