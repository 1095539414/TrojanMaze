using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SimpleZombie : Zombie {
    [SerializeField] int initialHealth = 100;
    [SerializeField] float initialSpeed = 0.7f;



    private Rigidbody2D _body;
    private NavMeshAgent _agent;
    private Vector2 _direction;

    private GameObject _prevHit;
    private bool _turnClockwise;

    private GameObject _target;

    private float _detectionRange = 2f;
    private float _followRange = 3f;
    float _idleTime;

    bool _idled;

    // Start is called before the first frame update
    void Start() {
        base.Init(initialHealth);
        _body = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _idled = false;
        _idleTime = 0f;
    }

    // Update is called once per frame
    void Update() {
        _direction = _agent.path.corners[0] - transform.position;
        // If it locks onto a player, it moves toward the player
        // if the player is too far, it loses the target
        if(_target) {
            if(_agent.remainingDistance > _followRange) {
                _target = null;
                _agent.SetDestination(transform.position);
                _agent.speed = initialSpeed;
            } else {
                _agent.SetDestination(_target.transform.position);
                _agent.speed = initialSpeed*2;
            }
        } else {
            
            RaycastHit2D hit = Physics2D.CircleCast((Vector2)transform.position + _direction, 0.5f, _direction);
            if(hit && hit.collider.CompareTag("Player") && hit.distance < _detectionRange) {
                _target = hit.collider.gameObject;
            } else {
                if(_agent.remainingDistance <= _agent.stoppingDistance) {
                    if(_idleTime <= 0) {
                        if(_idled) {
                            _idled = false;
                           Vector2 dest = GetRandomDest();
                            _agent.SetDestination(dest);
                            _agent.speed = initialSpeed;
                        } else {
                            _idled = true;
                            _idleTime = Random.Range(3, 6);
                        }
                    } else {
                        _idleTime -= Time.deltaTime;
                    }
                } 
            }
        }
    }

    private bool CanReach(Vector2 position) {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    private Vector2 GetRandomDest() {
        float r = 3f;
        float curX = transform.position.x;
        float curY = transform.position.y;
        Vector2 dest;
        int count = 0;
        do {
            dest = new Vector2(curX + Random.Range(-r, r), curY + Random.Range(-r, r));
            count++;
            if(count > 10) {
                Debug.LogWarning("Zombie Agent: Could not find a valid path to anywhere");
                break;
            }
        } while(!CanReach(dest));
        return dest;
    }
    private void OnCollisionEnter(Collision other) {
        if(other.collider.tag.Equals("Player")) {
            if(_target == null) {
                _target = other.gameObject;
            } else {
                _agent.SetDestination(transform.position);
            }
        }
    }
}
