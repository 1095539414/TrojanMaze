using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {
    private int _health;
    private float _speed;

    // Getters
    public int Health {
        get {return _health;} 
    }
    public float Speed {
        get {return _speed;} 
    }

    // used by its inheritance
    protected void Init(int health, float speed) {
        _health = health;
        _speed = speed;
        this.gameObject.tag = "Zombie";
    }

    // Health calculation 
    // other factors that are influenced by helath could be done here
    protected bool ReduceHealth(int amount) {
        _health -= amount;
        if(_health <= 0) {
            StartCoroutine(Die());
        }
        return true;
    }

    private IEnumerator Die() {
        // Death Animation here

        // wait for a bit before erasing the zombie
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
