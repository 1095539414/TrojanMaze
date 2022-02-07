using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleZombie : Zombie {
    [SerializeField] int InitialHealth = 100;
    [SerializeField] float InitialSpeed = 500f;

    private Rigidbody2D _body;
    private Vector2 _direction;
    // Start is called before the first frame update
    void Start() {
        base.Init(InitialHealth, InitialSpeed);
        _body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 movement = transform.right * Speed * Time.deltaTime;
        Debug.Log(transform.forward);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + transform.right, 0.15f, transform.right);
        if(hit) {
            Debug.Log(hit.collider.gameObject.tag);
            if(hit.distance < 2.0f) {
                float angle = Random.Range(-30, 30);
                _body.MoveRotation(angle*2*Time.deltaTime);
            }
        }
        //_body.velocity = movement;
    }
}
