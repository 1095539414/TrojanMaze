using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleZombie : Zombie {
    [SerializeField] int InitialHealth = 100;
    [SerializeField] float InitialSpeed = 750f;

    private Rigidbody2D _body;
    private Vector2 _direction;

    private GameObject prevHit;

    private bool turnClockWise;
    // Start is called before the first frame update
    void Start() {
        base.Init(InitialHealth, InitialSpeed);
        _body = GetComponent<Rigidbody2D>();
        turnClockWise = false;
        _direction = transform.right;
    }

    // Update is called once per frame
    void Update() {
        Vector2 movement = _direction * Speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast((Vector2)transform.position + _direction, 0.5f, _direction);
        bool keepMoving = true;
        // Detect if there is something in the front
        if(hit) {
            // if the object is really close and the object is not a player (most likely a wall)
            // stop moving and slowly turn away
            if(hit.distance < 1.0f && !hit.collider.CompareTag("Player")) {
                keepMoving = false;
                int angle = Random.Range(0, 70);

                float randomAction = Random.Range(0, 10);
                if(hit.collider.gameObject != prevHit && randomAction < 5) {
                    turnClockWise = !turnClockWise;
                }
                if(turnClockWise) {
                    angle *= -1;
                }
                Turn(angle);
            }
        }

        if(keepMoving) {
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
    }
}
