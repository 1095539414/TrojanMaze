using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpReduce : MonoBehaviour {
    private float _damage = 0.25f;
    [SerializeField] float moveSpeed = 0.02f;
    [SerializeField] bool horizontal = true;

    [SerializeField] Transform center;
    private Rigidbody2D _body;
    private Vector2 _initialPos;
    private bool _direction = true;
    private Vector3 _velocity;

    // Start is called before the first frame update
    void Start() {
        _body = GetComponent<Rigidbody2D>();
        _initialPos = transform.position;
        gameObject.layer = 7;
    }

    // Update is called once per frame
    void Update() {

        // virus zombies move horizontally 
        if(horizontal) {
            if(_direction) {
                _velocity = Vector3.right * moveSpeed;
            } else {
                _velocity = Vector3.left * moveSpeed;
            }
        // virus zombies move vertically 
        } else {
            if(_direction) {
                _velocity = Vector3.up * moveSpeed;
            } else {
                _velocity = Vector3.down * moveSpeed;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            iDamageable damageableObj = other.gameObject.GetComponent<iDamageable>();
            if(damageableObj != null) {
                damageableObj.ReduceHealth(_damage, this.gameObject);
            }
        }
        if(other.CompareTag("Walls")) {
            _direction = !_direction;
        }
    }

    void FixedUpdate() {
        _body.velocity = _velocity * Time.deltaTime;
    }
}

