using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleZombie : Zombie {
    [SerializeField] int InitialHealth = 100;
    [SerializeField] float InitialSpeed = 500f;

    private Rigidbody2D _body;
    private Vector2 _direction;

    private GameObject prevHit;

    private bool turnClockWise;
    // Start is called before the first frame update
    void Start() {
        base.Init(InitialHealth, InitialSpeed);
        _body = GetComponent<Rigidbody2D>();
        turnClockWise = false;
    }

    // Update is called once per frame
    void Update() {
        Vector2 movement = transform.right * Speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + transform.right, 0.5f, transform.right);
        bool keepMoving = true;
        // Detect if there is something in the front
        if(hit) {
            // if the object is really close and the object is not a player (most likely a wall)
            // stop moving and slowly turn away (zombie will look like its in idle state)
            if(hit.distance < 1.0f && !hit.collider.gameObject.CompareTag("Player")) {
                keepMoving = false;
                float angle = Random.Range(0, 70);

                float randomAction = Random.Range(0, 10);
                if(hit.collider.gameObject != prevHit && randomAction < 5) {
                    turnClockWise = !turnClockWise;
                }
                if(turnClockWise) {
                    angle *= -1;
                }

                Quaternion endRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + angle);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, endRotation, 50 * Time.deltaTime);
                prevHit = hit.collider.gameObject;
            } 
        }

        if(keepMoving) {
            _body.velocity = movement;
        } else {
            _body.velocity = new Vector2(0, 0);
        }
    }
}
