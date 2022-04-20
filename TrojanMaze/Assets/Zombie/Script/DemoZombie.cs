using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoZombie : Zombie {
    [SerializeField] float initialHealth = 1;
    private bool _dead = false;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start() {
        base.Init(initialHealth);
        gameObject.layer = 7;

        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

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
