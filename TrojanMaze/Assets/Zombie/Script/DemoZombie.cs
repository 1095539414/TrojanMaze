using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoZombie : Zombie {
    [SerializeField] float initialHealth = 1;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] bool canShoot;
    private bool _dead = false;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private float _fireTime;
    private GameObject _player;
    // Start is called before the first frame update
    void Start() {
        base.Init(initialHealth);
        gameObject.layer = 7;
        _player = GameObject.FindWithTag("Player");

        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if(canShoot && _player && Vector2.Distance(_player.transform.position, transform.position) <= 3f) {
            Vector3 _dirToPlayer = _player.transform.position - transform.position;
            _dirToPlayer.Normalize();
            // if there is no target and player appears in the zombies walking direction (90 degree vision)
            if(Vector2.Dot(_dirToPlayer, transform.right) >= Mathf.Cos(90 / 2)) {
                if(_fireTime < 0) {
                    StartCoroutine(Throwing());
                    _fireTime = Random.Range(4f, 7f);
                }
            }
            _fireTime -= Time.deltaTime;
        }
    }

    private IEnumerator Throwing() {
        Instantiate(bullet, gun.position, gun.rotation);
        _animator.SetBool("Throwing", true);
        yield return new WaitForSeconds(0.3f);
        _animator.SetBool("Throwing", false);
    }

    public override IEnumerator Die() {
        _dead = true;
        _animator.SetBool("Dead", true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(base.Die());
    }

    public override IEnumerator Hurt() {
        _animator.SetBool("Hurt", true);
        yield return new WaitForSeconds(0.3f);
        _animator.SetBool("Hurt", false);
        StartCoroutine(base.Hurt());
    }
}
