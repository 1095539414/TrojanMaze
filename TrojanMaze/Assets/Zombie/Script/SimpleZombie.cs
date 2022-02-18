using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SimpleZombie : Zombie {
    [SerializeField] int initialHealth = 1;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] float fireInterval = 3f;

    private Rigidbody2D _body;
    private NavMeshAgent _agent;

    private GameObject _target;
    private GameObject _player;
    private float _speed;
    private float _acceleration;
    private Vector2 _direction;
    private Vector2 _dirToPlayer;
    private float _attackDamage = 0.05f;
    private bool _attacking;
    private float _attackInterval = 0.5f;   // attack once every second
    private float _attackTime;

    private float _detectionRange = 3f;
    private float _followRange = 7f;
    private float _idleTime = 0f;
    private float _fireTime;



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
    }

    // Update is called once per frame
    void Update() {

        _fireTime += Time.deltaTime;
        // chase after the player if the zombie is targeting the player
        if(_attacking) {
            _attackTime += Time.deltaTime;
            if(_attackTime >= _attackInterval) {
                base.Attack(_player, _attackDamage);
                _attackTime = 0;
            }
        }
        else if(_target) {
            if(_fireTime >= fireInterval) {
                FireBullet();
                _fireTime = 0;
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

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.CompareTag("Player")) {
            if(_target == null) {
                _target = _player;
            }

            // prevent zombie from pushing players
            _agent.isStopped = true;
            _attacking = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.collider.CompareTag("Player")) {
            // keep chasing player
            _agent.isStopped = false;
            _attacking = false;
        }
    }

    void FireBullet() {
        //Debug.Log("FireBullet");
        Instantiate(bullet, gun.position, gun.rotation);
    }
}
