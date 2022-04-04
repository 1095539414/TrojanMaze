using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SimpleZombieState {
    Idle = 1,
    Walking = 2,
    Running = 3,
    Attacking = 4,
    Dying = 5,
    Shooting = 6
}

public class SimpleZombie : Zombie {
    [SerializeField] float initialHealth = 1;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    private Rigidbody2D _body;
    private NavMeshAgent _agent;

    private GameObject _target;
    private GameObject _player;
    private float _speed;
    private float _acceleration;
    private Vector2 _direction;
    private Vector2 _dirToPlayer;
    private float _attackDamage = 0.05f;
    private bool _attacking = false;
    private float _attackInterval = 0.5f;   // attack once every second
    private float _attackTime;

    private float _detectionRange = 3f;
    private float _followRange = 7f;
    private float _idleTime = 0f;
    float _fireInterval = 5f;
    private float _fireTime;

    private bool _dead = false;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    void Start() {
        base.Init(initialHealth);
        _body = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _speed = _agent.speed;
        _acceleration = _agent.acceleration;
        _player = GameObject.FindWithTag("Player");
        _attackTime = _attackInterval;

        // set a random destination to start
        moveTo(GetRandomDest(), _speed, _acceleration);
        _fireTime = 0f;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if(_dead) {
            _agent.velocity = Vector3.zero;
            _agent.isStopped = true;
            _attacking = false;
            _target = null;
            return;
        }
        _animator.SetBool("Attacking", _attacking);
        _animator.SetFloat("Speed", Vector3.Magnitude(_agent.velocity));
        if(_agent.velocity.x > 0) {
            Vector3 theScale = transform.localScale;
            if(theScale.x < 0) {
                theScale.x *= -1;
            }
            transform.localScale = theScale;
        } else if(_agent.velocity.x < 0) {
            Vector3 theScale = transform.localScale;
            if(theScale.x > 0) {
                theScale.x *= -1;
            }
            transform.localScale = theScale;
        }

        _fireTime += Time.deltaTime;
        // chase after the player if the zombie is targeting the player
        if(_attacking) {
            _attackTime += Time.deltaTime;
            if(_attackTime >= _attackInterval) {
                base.Attack(_player, _attackDamage);
                _attackTime = 0;
            }
        } else if(_target) {
            if(_fireTime >= _fireInterval) {
                StartCoroutine(Throwing());
                _fireTime = 0;
                _fireInterval = Random.Range(4f, 7f);
            }
            // stop chasing when player is too far
            moveTo(_target.transform.position, _speed * 2, _acceleration * 10);
            if(_agent.remainingDistance > _followRange) {
                _target = null;
                Idle();
            }
        } else if(_player && Vector2.Distance(_player.transform.position, transform.position) <= _detectionRange) {
            _dirToPlayer = _player.transform.position - transform.position;
            _dirToPlayer.Normalize();

            // if there is no target and player appears in the zombies walking direction (90 degree vision)
            if(Vector2.Dot(_dirToPlayer, _direction) >= Mathf.Cos(90 / 2)) {
                _target = _player;
                _agent.isStopped = false;
            }
        }

        // if it is in idle state, get ready for the next movement
        if(!_target && _agent.isStopped) {
            if(_idleTime <= 0) {
                _agent.isStopped = false;
                moveTo(GetRandomDest(), _speed, _acceleration);
            } else {
                _idleTime -= Time.deltaTime;
            }
        } else if(!_target && _agent.remainingDistance <= _agent.stoppingDistance) {
            Idle();
        }

        // get zombie's facing direction
        if(_agent.path.corners.Length > 1) {
            _direction = _agent.path.corners[1] - transform.position;
            _direction.Normalize();
        }
    }

    // stops the zombie for a short period of time
    // mimic zombie behavior
    private void Idle() {
        _agent.isStopped = true;
        _idleTime = Random.Range(2f, 5f);
    }

    // check if the agent can reach that position
    private bool CanReach(Vector2 position) {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    // set the agent destination with specified speed and acceleration
    private void moveTo(Vector2 dest, float speed, float acceleration) {
        _agent.SetDestination(dest);
        _agent.acceleration = acceleration;
        _agent.speed = speed;
    }

    // get a random destination around the zombie
    private Vector2 GetRandomDest() {
        float r = 2.5f;
        float curX = transform.position.x;
        float curY = transform.position.y;
        Vector2 dest;
        int count = 0;
        do {
            dest = new Vector2(curX + Random.Range(-r, r), curY + Random.Range(-r, r));
            count++;
            if(count > 10) {
                Debug.LogWarning("Simple Zombie: Could not find a valid destination to go to");
                break;
            }
        } while(!CanReach(dest) || Vector2.Distance(transform.position, dest) <= _agent.stoppingDistance);
        return dest;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            if(_target == null) {
                _target = _player;
            }

            // prevent zombie from pushing players
            _agent.isStopped = true;
            _attacking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            // keep chasing player
            _agent.isStopped = false;
            _attacking = false;
        }
    }

    public override IEnumerator Die() {
        _dead = true;

        _animator.SetBool("Dead", true);
        yield return new WaitForSeconds(2f);
        StartCoroutine(base.Die());
    }

    public override IEnumerator Hurt() {
        _agent.velocity = Vector3.zero;
        _animator.SetBool("Hurt", true);
        yield return new WaitForSeconds(0.3f);
        _animator.SetBool("Hurt", false);
        StartCoroutine(base.Hurt());
    }

    private IEnumerator Throwing() {
        _agent.velocity = Vector3.zero;
        Instantiate(bullet, gun.position, gun.rotation);
        _animator.SetBool("Throwing", true);
        yield return new WaitForSeconds(0.3f);
        _animator.SetBool("Throwing", false);
    }
}
