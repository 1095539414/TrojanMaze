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

    void Start() {
        base.Init(initialHealth);
        _body = GetComponent<Rigidbody2D>();
        _initialPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        _time += Time.deltaTime;
        // virus zombies move horizontally 
        if(horizontal) {
            if(_direction) {
                _body.MovePosition(transform.position + Vector3.right * moveSpeed);
                if(transform.position.x - _initialPos.x > 3f) {
                    _direction = false;
                }
            } else {
                _body.MovePosition(transform.position + Vector3.left * moveSpeed);
                if(transform.position.x - _initialPos.x < -3f) {
                    _direction = true;
                }
            }
        // virus zombies move vertically 
        } else {
            if(_direction) {
                _body.MovePosition(transform.position + Vector3.up * moveSpeed);
                if(transform.position.y - _initialPos.y > 3f) {
                    _direction = false;
                }
            } else {
                _body.MovePosition(transform.position + Vector3.down * moveSpeed);
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

    void ReleaseVirus() {
        Instantiate(VirusZone, center.position, center.rotation);
    }
}
