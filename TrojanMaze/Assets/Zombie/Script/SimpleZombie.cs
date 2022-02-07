using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleZombie : Zombie {
    [SerializeField] int InitialHealth = 100;
    [SerializeField] float InitialSpeed = 25f;


    private float _speed;
    private Rigidbody2D _body;
    private Vector2 _direction;
    private bool _keepMoving;
    private GameObject _prevHit;
    private bool _turnClockwise;


    // Start is called before the first frame update
    void Start() {
        base.Init(InitialHealth);
        _speed = InitialSpeed;
        _body = GetComponent<Rigidbody2D>();
        _turnClockwise = false;
        _direction = transform.right;
    }

    // Update is called once per frame
    void Update() {

        RaycastHit2D hit = Physics2D.CircleCast((Vector2)transform.position + _direction, 0.5f, _direction);
        _keepMoving = true;
        // Detect if there is something in the front
        if(hit) {
            // if the object is really close and the object is not a player (most likely a wall)
            // stop moving and slowly turn away
            if(hit.collider.CompareTag("Player")) {
                if(hit.distance < 2.0f) {
                    _speed = InitialSpeed * 2f;
                    _keepMoving = true;
                    _direction = hit.collider.gameObject.transform.position - transform.position;
                    _direction.Normalize();
                }
            } else if(hit.distance < .5f) {
                _speed = InitialSpeed;
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
