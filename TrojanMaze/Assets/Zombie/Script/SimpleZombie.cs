using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleZombie : Zombie {
    [SerializeField] int initialHealth = 100;
    [SerializeField] float initialSpeed = 25f;

    private float _speed;
    private Rigidbody2D _body;
    private Vector2 _direction;
    private bool _keepMoving;
    private GameObject _prevHit;
    private bool _turnClockwise;

    private GameObject _target;

    private float _detectionRange = 2f;
    private float _followRange = 3f;

    // Start is called before the first frame update
    void Start() {
        base.Init(initialHealth);
        _speed = initialSpeed;
        _body = GetComponent<Rigidbody2D>();
        _turnClockwise = false;
        _direction = transform.right;
    }

    // Update is called once per frame
    void Update() {
        // If it locks onto a player, it moves toward the player
        // if the player is too far, it loses the target
        if(_target) {
            if(Vector2.Distance(_target.transform.position, transform.position) >= _followRange) {
                _target = null;
            } else {
                _speed = initialSpeed * 2f;
                _keepMoving = true;
                _direction = _target.transform.position - transform.position;
                _direction.Normalize();
            }
        } else {
            // Normal behaviour, detecting walls and player in its eye sight.
            RaycastHit2D hit = Physics2D.CircleCast((Vector2)transform.position + _direction, 0.5f, _direction);
            _keepMoving = true;
            // Detect if there is something in the front
            if(hit) {
                // if the object is really close and the object is not a player (most likely a wall)
                // stop moving and slowly turn away
                if(hit.collider.CompareTag("Player")) {
                    if(hit.distance < _detectionRange) {
                        _target = hit.collider.gameObject;
                    }
                } else if(hit.distance < .5f) {
                    _speed = initialSpeed;
                    _keepMoving = false;
                    int angle = Random.Range(0, 70);

                    float randomAction = Random.Range(0, 10);
                    if(hit.collider.gameObject != _prevHit && randomAction < 5) {
                        _turnClockwise = !_turnClockwise;
                    }
                    if(_turnClockwise) {
                        angle *= -1;
                    }
                    Turn(angle);
                }
            }
        }
    }

    void FixedUpdate() {
        Vector2 movement = _direction * _speed * Time.deltaTime;
        if(_keepMoving) {
            _body.velocity = movement;
        } else {
            _body.velocity = new Vector2(0, 0);
        }
    }

    void Turn(int degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = _direction.x;
        float ty = _direction.y;
        _direction.x = (cos * tx) - (sin * ty);
        _direction.y = (sin * tx) + (cos * ty);
        _direction.Normalize();
    }
}
