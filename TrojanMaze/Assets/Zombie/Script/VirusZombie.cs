using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusZombie : Zombie {
    [SerializeField] int initialHealth = 2;
    [SerializeField] float moveSpeed = 0.02f;
    [SerializeField] float releaseInterval = 0.2f;
    [SerializeField] bool horizontal = true;

    [SerializeField] GameObject VirusZone;
    [SerializeField] Transform center;
    private Rigidbody2D _body;
    private GameObject _player;
    private Vector2 _initialPos;
    private bool _direction = true;
    private float _time = 0f;
    private Animator _animator;
    private Vector3 _velocity;
    void Start() {
        base.Init(initialHealth);
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _initialPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if(_body.velocity.x > 0) {
            Vector3 theScale = transform.localScale;
            if(theScale.x < 0) {
                theScale.x *= -1;
            }
            transform.localScale = theScale;
        } else if(_body.velocity.x < 0) {
            Vector3 theScale = transform.localScale;
            if(theScale.x > 0) {
                theScale.x *= -1;
            }
            transform.localScale = theScale;
        }

        _animator.SetFloat("Speed", Vector3.Magnitude(_body.velocity));
        _time += Time.deltaTime;
        // virus zombies move horizontally 
        if(horizontal) {
            if(_direction) {
                _velocity = Vector3.right * moveSpeed;
                if(transform.position.x - _initialPos.x > 3f) {
                    _direction = false;
                }
            } else {
                _velocity = Vector3.left * moveSpeed;
                if(transform.position.x - _initialPos.x < -3f) {
                    _direction = true;
                }
            }
        // virus zombies move vertically 
        } else {
            if(_direction) {
                _velocity = Vector3.up * moveSpeed;
                if(transform.position.y - _initialPos.y > 3f) {
                    _direction = false;
                }
            } else {
                _velocity = transform.position + Vector3.down * moveSpeed;
                if(transform.position.y - _initialPos.y < -3f) {
                    _direction = true;
                }
            }
        }
        if(_time >= releaseInterval) {
            ReleaseVirus();
            _time = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Walls")) {
            _direction = !_direction;
        }
    }

    void FixedUpdate() {
        _body.velocity = _velocity * Time.deltaTime;
    }

    void ReleaseVirus() {
        Instantiate(VirusZone, center.position, center.rotation);
    }
}
