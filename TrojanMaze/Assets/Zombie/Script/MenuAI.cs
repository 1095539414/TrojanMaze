using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MenuAI : MonoBehaviour {
    private Rigidbody2D _body;
    private NavMeshAgent _agent;
    private Animator _animator;
    private float _idleTime = 0f;
    private float _speed;
    private float _acceleration;
    // Start is called before the first frame update
    void Start() {
        _body = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _speed = _agent.speed;
        _acceleration = _agent.acceleration;
        moveTo(GetRandomDest(), _speed, _acceleration);

    }

    // Update is called once per frame
    void Update() {
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
        if(_agent.isStopped) {
            if(_idleTime <= 0) {
                _agent.isStopped = false;
                if(Random.value < 0.4) {
                    moveTo(GetRandomDest(), _speed * 2f, _acceleration * 10);
                } else {
                    moveTo(GetRandomDest(), _speed, _acceleration);
                }
            } else {
                _idleTime -= Time.deltaTime;
            }
        } else if(_agent.remainingDistance <= _agent.stoppingDistance) {
            Idle();
        }
    }

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

}
