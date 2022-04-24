using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpReduce : MonoBehaviour {
    private float _damage = 0.2f;
    [SerializeField] float moveSpeed = 0.02f;
    [SerializeField] bool horizontal = true;

    [SerializeField] Transform center;
    public Sprite[] AnimationSprites;
    private Rigidbody2D _body;
    private Vector2 _initialPos;
    private bool _direction = true;
    private Vector3 _velocity;
    private float _time;

    private SpriteRenderer _renderer;
    // Start is called before the first frame update
    void Start() {
        _body = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _initialPos = transform.position;
        gameObject.layer = 7;
    }

    // Update is called once per frame
    void Update() {
        // virus zombies move horizontally 
        if(horizontal) {
            Vector2 scale = transform.localScale;
            scale.x = Mathf.Sign(-1 * _velocity.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
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
        if(_time <= 0) {
            StartCoroutine(Animate());
            _time = Random.Range(1.5f, 2.5f) / (moveSpeed / 80f);
        }
        _time -= Time.deltaTime;
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

    IEnumerator Animate() {
        _renderer.sprite = AnimationSprites[1];
        yield return new WaitForSeconds(0.2f);
        _renderer.sprite = AnimationSprites[0];

    }

    void FixedUpdate() {
        _body.velocity = _velocity * Time.deltaTime;
    }
}

